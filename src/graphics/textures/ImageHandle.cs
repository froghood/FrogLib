
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public struct ImageHandle {

    public long Handle => handle;

    private long handle;

    public ImageHandle(long handle) => this.handle = handle;

    public void MakeResident(TextureAccess access) => GL.Arb.MakeImageHandleResident(handle, (All)access);
    public void MakeNonResident() => GL.Arb.MakeImageHandleNonResident(handle);
}