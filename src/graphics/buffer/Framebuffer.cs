using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class Framebuffer : GLObject {

    public Vec2i Size => size;


    private Vec2i size;
    private FastStack<TextureAttachment> textures = new();
    private FastStack<RenderbufferAttachment> renderbuffers = new();
    private Dictionary<FramebufferAttachment, AttachmentInfo> attachmentInfo = new();


    public Framebuffer(Vec2i size)
    : base(GL.CreateFramebuffer()) {
        this.size = size;
    }

    public void Use() {
        ThrowIfInvalid();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
    }

    public void SetDrawBuffers(params DrawBuffersEnum[] drawBuffers) {
        ThrowIfInvalid();
        GL.NamedFramebufferDrawBuffers(Id, drawBuffers.Length, drawBuffers);
    }

    public void SetReadBuffer(ReadBufferMode mode) {
        ThrowIfInvalid();
        GL.NamedFramebufferReadBuffer(Id, mode);
    }

    public void Blit(Framebuffer other, ClearBufferMask mask, BlitFramebufferFilter filter) {
        ThrowIfInvalid();
        other.ThrowIfInvalid();
        GL.BlitNamedFramebuffer(Id, other.Id, 0, 0, size.X, size.Y, 0, 0, size.X, size.Y, mask, filter);
    }

    public void Resize(Vec2i size) {

        ThrowIfInvalid();

        size = Vec2i.Max(size, Vec2i.One);

        for (int i = 0; i < textures.Length; i++) {
            ref var previous = ref textures[i];
            var texture = previous.Copy(size);
            previous.Dispose();
            previous = texture;

            texture.Attach(Id);
        }

        for (int i = 0; i < renderbuffers.Length; i++) {
            ref var previous = ref renderbuffers[i];
            var renderbuffer = previous.Copy(size);
            previous.Dispose();
            previous = renderbuffer;

            renderbuffer.Attach(Id);
        }

        this.size = size;
    }



    public void UseTexture(FramebufferAttachment type, int unit) {

        ThrowIfInvalid();

        if (!attachmentInfo.TryGetValue(type, out var info) || info.Type != AttachmentType.Texture) {
            throw new InvalidOperationException($"Cannot use texture; no texture is attached to {type}.");
        }

        textures[info.Index].Use(unit);
    }

    public int GetTextureId(FramebufferAttachment type) {

        ThrowIfInvalid();

        if (!attachmentInfo.TryGetValue(type, out var info) || info.Type != AttachmentType.Texture) {
            throw new InvalidOperationException($"Cannot get texture Id; no texture is attached to {type}.");
        }

        return textures[info.Index].Id;
    }

    public void UseImage(FramebufferAttachment type, int unit, TextureAccess access) {

        ThrowIfInvalid();

        if (!attachmentInfo.TryGetValue(type, out var info) || info.Type != AttachmentType.Texture) {
            throw new InvalidOperationException($"Cannot use image; no texture is attached to {type}.");
        }

        textures[info.Index].UseImage(unit, access);
    }

    public void AttachTexture(FramebufferAttachment type, SizedInternalFormat format, params TextureParameter[] textureParameters) {

        ThrowIfInvalid();

        var attachment = new TextureAttachment(size, type, format, textureParameters);
        attachment.Attach(Id);

        if (attachmentInfo.TryGetValue(type, out var info)) {

            if (info.Type == AttachmentType.Renderbuffer) {
                renderbuffers.Remove(info.Index).Dispose();
            }

            ref var texture = ref textures[info.Index];
            texture.Dispose();
            texture = attachment;

            return;
        }

        attachmentInfo[type] = new AttachmentInfo {
            Type = AttachmentType.Texture,
            Index = textures.Push(attachment)
        };
    }

    public void AttachRenderbuffer(FramebufferAttachment type, RenderbufferStorage storage) {

        ThrowIfInvalid();

        var attachment = new RenderbufferAttachment(size, type, storage);
        attachment.Attach(Id);

        if (attachmentInfo.TryGetValue(type, out var info)) {

            if (info.Type == AttachmentType.Texture) {
                textures.Remove(info.Index).Dispose();
            }

            ref var renderbuffer = ref renderbuffers[info.Index];
            renderbuffer.Dispose();
            renderbuffer = attachment;

            return;
        }

        attachmentInfo[type] = new AttachmentInfo {
            Type = AttachmentType.Renderbuffer,
            Index = renderbuffers.Push(attachment)
        };
    }

    protected override void Delete() {
        for (int i = 0; i < textures.Length; i++) textures[i].Dispose();
        for (int i = 0; i < renderbuffers.Length; i++) renderbuffers[i].Dispose();
        GL.DeleteFramebuffer(Id);
    }

    private struct AttachmentInfo {
        public AttachmentType Type;
        public int Index;
    }

    private enum AttachmentType {
        Texture,
        Renderbuffer
    }
}




