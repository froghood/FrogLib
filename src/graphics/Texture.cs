using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace FrogLib;

public class Texture {

    public int Id { get; }
    public Vector2i Size { get; }


    private SizedInternalFormat format;



    public Texture(string path, bool preMultiply = false, bool verticalFlip = true) {
        GL.CreateTextures(TextureTarget.Texture2D, 1, out int id);
        Id = id;

        format = SizedInternalFormat.Rgba8;

        using var stream = File.OpenRead(path);

        StbImage.stbi_set_flip_vertically_on_load(verticalFlip ? 1 : 0);

        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        Size = new Vector2i(image.Width, image.Height);

        GL.TextureStorage2D(id, 1, format, image.Width, image.Height);

        if (preMultiply) PreMultiply(image);

        GL.TextureSubImage2D(id, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
    }

    public Texture(int width, int height, SizedInternalFormat format) {
        GL.CreateTextures(TextureTarget.Texture2D, 1, out int id);
        Id = id;

        Size = new Vector2i(width, height);

        this.format = format;

        GL.TextureStorage2D(id, 1, format, width, height);
    }

    public Texture(Vector2i size, SizedInternalFormat format) : this(size.X, size.Y, format) { }



    public void Use(int unit) {
        GL.BindTextureUnit(unit, Id);
    }

    public void UseImage(int unit, TextureAccess access) {
        GL.BindImageTexture(unit, Id, 0, false, 0, access, format);
    }

    public void SetParam(TextureParameterName param, int value) => GL.TextureParameter(Id, param, value);
    public void SetParam(TextureParameterName param, float value) => GL.TextureParameter(Id, param, value);
    public unsafe void SetParam(TextureParameterName param, Vector4 value) => GL.TextureParameter(Id, param, (float*)value);
    public unsafe void SetParam(TextureParameterName param, Color4 value) => GL.TextureParameter(Id, param, (float*)((Vector4)value));

    public BindlessTexture MakeBindless() => new BindlessTexture(this);

    private static void PreMultiply(ImageResult image) {
        for (int i = 0; i < image.Data.Length; i += 4) {

            float a = image.Data[i + 3] / 255f;

            image.Data[i + 0] = (byte)MathF.Round(image.Data[i + 0] * a);
            image.Data[i + 1] = (byte)MathF.Round(image.Data[i + 1] * a);
            image.Data[i + 2] = (byte)MathF.Round(image.Data[i + 2] * a);

        }
    }
}