using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace FrogLib;

public struct Texture2d : ITexture {

    public int Id => id;
    public TextureTarget Target => (TextureTarget)target;
    public int Levels => levels;
    public Vec2i Size => size;



    private TextureTarget2d target;
    private int levels;
    private SizedInternalFormat format;
    private Vec2i size;
    private int id;

    private bool isHandleLocked = false;

    public Texture2d(TextureTarget2d target, int levels, Vec2i size, SizedInternalFormat format) {
        this.target = target;
        this.levels = levels;
        this.format = format;
        this.size = size;

        GL.CreateTextures((TextureTarget)target, 1, out id);
        GL.TextureStorage2D(id, levels, format, size.X, size.Y);
    }

    public static Texture2d FromFile(string path, bool preMultiply = false, bool verticalFlip = true) {

        using var file = File.OpenRead(path);

        StbImage.stbi_set_flip_vertically_on_load(verticalFlip ? 1 : 0);

        var image = ImageResult.FromStream(file, ColorComponents.RedGreenBlueAlpha);

        if (preMultiply) PreMultiply(image);

        var texture = new Texture2d(TextureTarget2d.Texture2D, 1, new Vec2i(image.Width, image.Height), SizedInternalFormat.Rgba8);

        texture.SetData(image.Data, Vec2i.Zero, new Vec2i(image.Width, image.Height), PixelFormat.Rgba);

        return texture;

        static void PreMultiply(ImageResult image) {
            for (int i = 0; i < image.Data.Length; i += 4) {
                float a = image.Data[i + 3] / 255f;
                image.Data[i + 0] = (byte)MathF.Round(image.Data[i + 0] * a);
                image.Data[i + 1] = (byte)MathF.Round(image.Data[i + 1] * a);
                image.Data[i + 2] = (byte)MathF.Round(image.Data[i + 2] * a);
            }
        }
    }

    public void SetData(byte[] data, int level, Vec2i offset, Vec2i size, PixelFormat format) {
        if (level < 0 || level >= levels) throw new TextureLevelOutOfRangeException();

        if (offset.X < 0 ||
            offset.Y < 0 ||
            offset.X + size.X > this.size.X ||
            offset.Y + size.Y > this.size.Y)
            throw new TextureDataOutOfBoundsException();

        GL.TextureSubImage2D(id, level, offset.X, offset.Y, size.X, size.Y, format, PixelType.UnsignedByte, data);
    }

    public void SetData(byte[] data, Vec2i offset, Vec2i size, PixelFormat format) {
        SetData(data, 0, offset, size, format);
        GL.GenerateTextureMipmap(id);
    }

    public void Use(int unit) {
        GL.BindTextureUnit(unit, id);
    }

    public void UseImage(int unit, TextureAccess access) {
        GL.BindImageTexture(unit, id, 0, false, 0, access, format);
    }

    public void SetParam(TextureParameterName param, int value) {
        ThrowIfHandleLocked();
        GL.TextureParameter(id, param, value);
    }
    public void SetParam(TextureParameterName param, float value) {
        ThrowIfHandleLocked();
        GL.TextureParameter(id, param, value);
    }
    public unsafe void SetParam(TextureParameterName param, Vec4 value) {
        ThrowIfHandleLocked();
        GL.TextureParameter(id, param, (float*)&value);
    }
    public unsafe void SetParam(TextureParameterName param, Color4 value) {
        ThrowIfHandleLocked();
        GL.TextureParameter(id, param, (float*)&value);
    }

    public TextureHandle CreateHandle() {
        isHandleLocked = true;
        return new TextureHandle(GL.Arb.GetTextureHandle(id));
    }

    public ImageHandle CreateImageHandle(int level, bool layered, int layer, PixelFormat format) {
        isHandleLocked = true;
        return new ImageHandle(GL.Arb.GetImageHandle(id, level, layered, layer, format));
    }

    private void ThrowIfHandleLocked() {
        if (isHandleLocked) throw new ImmutableTextureException("A handle has previously been created for this texture.");
    }

    public void Dispose() {
        GL.DeleteTexture(id);
    }
}