using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;

using FramebufferAttachmentTarget = OpenTK.Graphics.OpenGL4.FramebufferAttachment;

namespace FrogLib;

internal class TextureAttachment : GLObject {

    public SizedInternalFormat Format => format;
    public TextureParameter[] Parameters => parameters;

    private FramebufferAttachmentTarget type;

    private SizedInternalFormat format;
    private TextureParameter[] parameters;



    public TextureAttachment(Vec2i size, FramebufferAttachmentTarget type, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(TextureTarget.Texture2D)) {

        this.type = type;
        this.format = format;
        this.parameters = parameters;

        GL.TextureStorage2D(Id, 1, format, size.X, size.Y);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Id);
        }
    }

    public void Use(int unit) {
        ThrowIfInvalid();
        GL.BindTextureUnit(unit, Id);
    }

    public void UseImage(int unit, TextureAccess access) {
        ThrowIfInvalid();
        GL.BindImageTexture(unit, Id, 0, false, 0, access, format);
    }


    internal void Attach(int fbo) {
        ThrowIfInvalid();
        GL.NamedFramebufferTexture(fbo, type, Id, 0);
    }

    internal TextureAttachment Copy(Vec2i size) {
        ThrowIfInvalid();

        return new TextureAttachment(size, type, format, parameters);
    }

    protected override void Delete() {
        GL.DeleteTexture(Id);
    }
}