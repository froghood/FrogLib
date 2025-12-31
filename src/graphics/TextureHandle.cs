
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public struct TextureHandle {

    public long Handle => Handle;

    private long handle;

    public TextureHandle(long handle) => this.handle = handle;

    public void MakeResident() => GL.Arb.MakeTextureHandleResident(handle);
    public void MakeNonResident() => GL.Arb.MakeTextureHandleNonResident(handle);
}