using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace FrogLib;

public struct Texture1d : ITexture {

    public int Id => id;
    public TextureTarget Target => (TextureTarget)target;
    public int Levels => levels;
    public int Size => size;



    private TextureTarget1d target;
    private int levels;
    private SizedInternalFormat format;
    private int size;

    private int id;
    private bool isHandleLocked = false;

    public Texture1d(TextureTarget1d target, int levels, int size, SizedInternalFormat format) {
        this.target = target;
        this.levels = levels;
        this.size = size;
        this.format = format;

        GL.CreateTextures((TextureTarget)target, 1, out id);
        GL.TextureStorage1D(id, levels, format, size);
    }

    public void SetData(byte[] data, int level, int offset, int size, PixelFormat format) {
        if (level < 0 || level >= levels) throw new TextureLevelOutOfRangeException();

        if (offset < 0 || offset + size > this.size) throw new TextureDataOutOfBoundsException();

        GL.TextureSubImage1D(id, level, offset, size, format, PixelType.UnsignedByte, data);
    }

    public void SetData(byte[] data, int offset, int size, PixelFormat format) {
        SetData(data, 0, offset, size, format);
        GL.GenerateTextureMipmap(id);
    }

    public void Use(int unit) {
        GL.BindTextureUnit(unit, id);
    }

    public void UseImage(int unit, TextureAccess access) {
        GL.BindImageTexture(unit, id, 0, false, 0, access, format);
    }

    public void SetParam(in TextureParameter parameter) {
        if (isHandleLocked) throw new ImmutableTextureException("A handle has previously been created for this texture.");
        parameter.Apply(id);
    }

    public TextureHandle CreateHandle() {
        isHandleLocked = true;
        return new TextureHandle(GL.Arb.GetTextureHandle(id));
    }

    public ImageHandle CreateImageHandle(int level, bool layered, int layer, PixelFormat format) {
        isHandleLocked = true;
        return new ImageHandle(GL.Arb.GetImageHandle(id, level, layered, layer, format));
    }

    public void Dispose() {
        GL.DeleteTexture(id);
    }
}