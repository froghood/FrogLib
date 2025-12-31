using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public interface ITexture : IDisposable {
    public int Id { get; }
    public TextureTarget Target { get; }

    public void Use(int unit);
    public void UseImage(int unit, TextureAccess access);

    public void SetParam(TextureParameterName name, int value);
    public void SetParam(TextureParameterName name, float value);
    public void SetParam(TextureParameterName name, Vec4 value);
    public void SetParam(TextureParameterName name, Color4 value);
    public TextureHandle CreateHandle();
    public ImageHandle CreateImageHandle(int level, bool layered, int layer, PixelFormat format);
}