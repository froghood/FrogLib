using System.Runtime.CompilerServices;
using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace FrogLib;

public class Texture2d : Texture {


    public Vec2i Size => size;

    private Vec2i size;

    public Texture2d(TextureTarget2d target, int levels, Vec2i size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(
        (TextureTarget)target,
        levels,
        size,
        format,
        parameters
    ) {

        this.size = size;
    }

    public static Texture2d FromFile(string path, int levels, bool preMultiply = false, bool verticalFlip = true) {

        using var file = File.OpenRead(path);

        StbImage.stbi_set_flip_vertically_on_load(verticalFlip ? 1 : 0);

        var image = ImageResult.FromStream(file, ColorComponents.RedGreenBlueAlpha);

        if (preMultiply) PreMultiply(image);

        var texture = new Texture2d(TextureTarget2d.Texture2D, levels, new Vec2i(image.Width, image.Height), SizedInternalFormat.Rgba8);

        texture.SetData(image.Data, new Box2i(Vec2i.Zero, new Vec2i(image.Width, image.Height)), PixelFormat.Rgba, PixelType.UnsignedByte);

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

    public unsafe void SetData<T>(ReadOnlySpan<T> data, int level, in Box2i subregion, PixelFormat format, PixelType type) where T : unmanaged {

        ThrowIfInvalid();

        if (level < 0 || level >= Levels) throw new ArgumentException($"Mipmap level {level} is outside the texture's available mipmap range: (0 - {Levels}).");

        var bounds = new Box2i(Vec2i.Zero, size);
        if (!bounds.FullyContains(subregion)) throw new ArgumentException("Attempted to set data outside the texture's bounds.");

        fixed (T* ptr = data) {
            GL.TextureSubImage2D(Id, level, subregion.Min.X, subregion.Min.Y, subregion.Size.X, subregion.Size.Y, format, type, (nint)ptr);
        }
    }

    /// <summary>
    /// Sets the texture data at level 0 and generates the mipmaps.
    /// </summary>
    public void SetData<T>(ReadOnlySpan<T> data, in Box2i subregion, PixelFormat format, PixelType type) where T : unmanaged {
        SetData(data, 0, subregion, format, type);
        GL.GenerateTextureMipmap(Id);
    }
}