using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public struct BindlessTexture {

    public long Handle { get; }
    public Vector2i Size { get; }



    internal BindlessTexture(Texture texture) {
        Handle = GL.Arb.GetTextureHandle(texture.Id);
        Size = texture.Size;
    }



    public void MakeResident() => GL.Arb.MakeTextureHandleResident(Handle);
    public void MakeNonResident() => GL.Arb.MakeTextureHandleNonResident(Handle);



    public static BindlessTexture Empty => default;
    public static BindlessTexture FromTexture(Texture texture) => new(texture);

}