using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;

using FramebufferAttachmentTarget = OpenTK.Graphics.OpenGL4.FramebufferAttachment;

namespace FrogLib;

internal class RenderbufferAttachment : GLObject {

    private FramebufferAttachmentTarget type;
    private RenderbufferStorage storage;



    public RenderbufferAttachment(Vec2i size, FramebufferAttachmentTarget type, RenderbufferStorage storage)
    : base(GL.CreateRenderbuffer()) {
        this.type = type;
        this.storage = storage;

        GL.NamedRenderbufferStorage(Id, storage, size.X, size.Y);
    }

    internal void Attach(int fbo) {
        GL.NamedFramebufferRenderbuffer(fbo, type, RenderbufferTarget.Renderbuffer, Id);
    }

    internal RenderbufferAttachment Copy(Vec2i size) {
        ThrowIfInvalid();

        return new RenderbufferAttachment(size, type, storage);
    }

    protected override void Delete() {
        GL.DeleteRenderbuffer(Id);
    }
}