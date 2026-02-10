using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class MultisampleFramebuffer : GLObject {

    public Vec2i Size => size;
    public int Samples => samples;

    private Vec2i size;
    private int samples;

    private FastStack<TextureAttachment> textures = new();
    private FastStack<RenderbufferAttachment> renderbuffers = new();
    private Dictionary<FramebufferAttachment, AttachmentInfo> attachmentInfo = new();



    public MultisampleFramebuffer(Vec2i size, int samples)
    : base(GL.CreateFramebuffer()) {
        this.size = size;
        if (samples <= 0) throw new ArgumentException("Samples must be greater than 0.");
        int maxSamples = GL.GetInteger(GetPName.MaxSamples);
        if (samples > maxSamples) throw new ArgumentException($"Samples must be less than or equal to the current maximum supported samples: {maxSamples}.");
        this.samples = samples;
    }



    public void AttachTexture(FramebufferAttachment attachment, SizedInternalFormat format, params TextureParameter[] textureParameters) {
        ThrowIfInvalid();

        if (attachmentInfo.TryGetValue(attachment, out var info)) {
            if (info.Type == AttachmentType.Renderbuffer) {
                renderbuffers.Remove(info.Index).Delete();
            }

            ref var previous = ref textures[info.Index];
            previous.Delete();
            previous = new TextureAttachment(Handle, size, samples, attachment, format, textureParameters);

            return;
        }

        attachmentInfo[attachment] = new AttachmentInfo {
            Type = AttachmentType.Texture,
            Index = textures.Push(new TextureAttachment(Handle, size, samples, attachment, format, textureParameters))
        };
    }

    public void AttachRenderbuffer(FramebufferAttachment attachment, RenderbufferStorage storage) {
        ThrowIfInvalid();

        if (attachmentInfo.TryGetValue(attachment, out var info)) {
            if (info.Type == AttachmentType.Texture) {
                textures.Remove(info.Index).Delete();
            }

            ref var previous = ref renderbuffers[info.Index];
            previous.Delete();
            previous = new RenderbufferAttachment(Handle, size, samples, attachment, storage);

            return;
        }

        attachmentInfo[attachment] = new AttachmentInfo {
            Type = AttachmentType.Renderbuffer,
            Index = renderbuffers.Push(new RenderbufferAttachment(Handle, size, samples, attachment, storage))
        };
    }



    public void Use() {
        ThrowIfInvalid();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
    }



    public void Resize(Vec2i size) {
        ThrowIfInvalid();

        size = Vec2i.Max(size, Vec2i.One);

        for (int i = 0; i < textures.Length; i++) {
            ref var previous = ref textures[i];
            previous.Delete();
            previous = new TextureAttachment(Handle, size, samples, previous.Attachment, previous.Format, previous.Parameters);
        }

        for (int i = 0; i < renderbuffers.Length; i++) {
            ref var previous = ref renderbuffers[i];
            previous.Delete();
            previous = new RenderbufferAttachment(Handle, size, samples, previous.Attachment, previous.Storage);
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

    public void UseTexture(FramebufferAttachment type, int unit, Sampler sampler) {
        UseTexture(type, unit);
        sampler.Use(unit);
    }

    public void UseImage(FramebufferAttachment type, int unit, TextureAccess access) {
        ThrowIfInvalid();

        if (!attachmentInfo.TryGetValue(type, out var info) || info.Type != AttachmentType.Texture) {
            throw new InvalidOperationException($"Cannot use image; no texture is attached to {type}.");
        }

        textures[info.Index].UseImage(unit, access);
    }



    public int GetTextureHandle(FramebufferAttachment attachment) {

        ThrowIfInvalid();

        if (!attachmentInfo.TryGetValue(attachment, out var info) || info.Type != AttachmentType.Texture) {
            throw new InvalidOperationException($"Cannot get texture handle; no texture is attached to {attachment}.");
        }

        return textures[info.Index].Handle;
    }



    public void SetDrawBuffers(params DrawBuffersEnum[] drawBuffers) {
        ThrowIfInvalid();
        GL.NamedFramebufferDrawBuffers(Handle, drawBuffers.Length, drawBuffers);
    }

    public void SetReadBuffer(ReadBufferMode mode) {
        ThrowIfInvalid();
        GL.NamedFramebufferReadBuffer(Handle, mode);
    }

    public void Blit(MultisampleFramebuffer other, ClearBufferMask mask, BlitFramebufferFilter filter) {
        ThrowIfInvalid();
        other.ThrowIfInvalid();
        GL.BlitNamedFramebuffer(Handle, other.Handle, 0, 0, size.X, size.Y, 0, 0, size.X, size.Y, mask, filter);
    }

    public void Blit(Framebuffer other, ClearBufferMask mask, BlitFramebufferFilter filter) {
        ThrowIfInvalid();
        other.ThrowIfInvalid();
        GL.BlitNamedFramebuffer(Handle, other.Handle, 0, 0, size.X, size.Y, 0, 0, size.X, size.Y, mask, filter);
    }



    protected override void Delete() {
        for (int i = 0; i < textures.Length; i++) textures[i].Delete();
        for (int i = 0; i < renderbuffers.Length; i++) renderbuffers[i].Delete();
        GL.DeleteFramebuffer(Handle);
    }



    private enum AttachmentType {
        Texture,
        Renderbuffer
    }

    private struct AttachmentInfo {
        public AttachmentType Type;
        public int Index;
    }

    private struct TextureAttachment {

        public int Handle => handle;
        public FramebufferAttachment Attachment => attachment;
        public SizedInternalFormat Format => format;
        public TextureParameter[] Parameters => parameters;

        private int handle;
        private FramebufferAttachment attachment;
        private SizedInternalFormat format;
        private TextureParameter[] parameters;

        public TextureAttachment(int fbo, Vec2i size, int samples, FramebufferAttachment attachment, SizedInternalFormat format, params TextureParameter[] parameters) {
            handle = GL.CreateTexture(TextureTarget.Texture2DMultisample);
            this.attachment = attachment;
            this.format = format;
            this.parameters = parameters;
            GL.TextureStorage2DMultisample(handle, samples, format, size.X, size.Y, true);

            for (int i = 0; i < parameters.Length; i++) {
                parameters[i].Apply(handle);
            }

            GL.NamedFramebufferTexture(fbo, attachment, handle, 0);
        }

        public void Use(int unit) {
            GL.BindTextureUnit(unit, handle);
        }

        public void UseImage(int unit, TextureAccess access) {
            GL.BindImageTexture(unit, handle, 0, false, 0, access, format);
        }

        public void Delete() {
            GL.DeleteTexture(handle);
        }
    }

    private struct RenderbufferAttachment {

        public FramebufferAttachment Attachment => attachment;
        public RenderbufferStorage Storage => storage;

        private int handle;
        private FramebufferAttachment attachment;
        private RenderbufferStorage storage;

        public RenderbufferAttachment(int fbo, Vec2i size, int samples, FramebufferAttachment attachment, RenderbufferStorage storage) {
            handle = GL.CreateRenderbuffer();
            this.attachment = attachment;
            this.storage = storage;
            GL.NamedRenderbufferStorageMultisample(handle, samples, storage, size.X, size.Y);
            GL.NamedFramebufferRenderbuffer(fbo, attachment, RenderbufferTarget.Renderbuffer, handle);
        }

        public void Delete() {
            GL.DeleteRenderbuffer(handle);
        }
    }
}