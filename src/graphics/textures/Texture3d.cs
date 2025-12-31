using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace FrogLib;

public struct Texture3d : ITexture {

    public int Id => id;
    public TextureTarget Target => (TextureTarget)target;
    public int Levels => levels;
    public Vec3i Size => size;


    private int id;
    private TextureTarget3d target;
    private int levels;
    private SizedInternalFormat format;
    private Vec3i size;

    private bool isHandleLocked = false;

    public Texture3d(TextureTarget3d target, int levels, Vec3i size, SizedInternalFormat format) {
        this.target = target;
        this.levels = levels;
        this.format = format;
        this.size = size;

        GL.CreateTextures((TextureTarget)target, 1, out id);
        GL.TextureStorage3D(id, levels, format, size.X, size.Y, size.Z);
    }

    public void SetData(byte[] data, int level, Vec3i offset, Vec3i size, PixelFormat format) {
        if (level < 0 || level >= levels) throw new TextureLevelOutOfRangeException();

        if (offset.X < 0 ||
            offset.Y < 0 ||
            offset.Z < 0 ||
            offset.X + size.X > this.size.X ||
            offset.Y + size.Y > this.size.Y ||
            offset.Z + size.Z > this.size.Z)
            throw new TextureDataOutOfBoundsException();

        GL.TextureSubImage3D(id, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, format, PixelType.UnsignedByte, data);
    }

    public void SetData(byte[] data, Vec3i offset, Vec3i size, PixelFormat format) {
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

