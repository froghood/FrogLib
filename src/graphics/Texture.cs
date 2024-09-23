using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace FrogLib;

public class Texture {

    public static int Load(string imagePath, bool preMultiply) {

        int texture = GL.GenTexture();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, texture);

        //StbImage.stbi_set_flip_vertically_on_load(1);

        using (var stream = File.OpenRead(imagePath)) {
            var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            //pre-multiply
            if (preMultiply) {
                for (int i = 0; i < image.Data.Length; i += 4) {

                    // convert alpha to range of 0 - 1
                    float a = image.Data[i + 3] / 255f;

                    image.Data[i + 0] = (byte)MathF.Round(image.Data[i + 0] * a);
                    image.Data[i + 1] = (byte)MathF.Round(image.Data[i + 1] * a);
                    image.Data[i + 2] = (byte)MathF.Round(image.Data[i + 2] * a);

                }
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 0f, 0f, 0f, 0f });

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        return texture;
    }

}