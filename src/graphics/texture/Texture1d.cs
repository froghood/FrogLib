using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class Texture1d : Texture {

    public Texture1d(TextureTarget1d target, int levels, int size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base((TextureTarget)target, levels, size, format, parameters) { }

    public unsafe void SetData<T>(ReadOnlySpan<T> data, int level, int offset, int size, PixelFormat format, PixelType type) where T : unmanaged {

        ThrowIfInvalid();

        if (level < 0 || level >= Levels) throw new ArgumentException($"Mipmap Level {level} is outside the texture's available mipmap range: (0 - {Levels}).");

        if (offset < 0 || offset + size > Size.X)
            throw new ArgumentException("Attempted to set data outside the texture's bounds.");

        fixed (T* ptr = data) {
            GL.TextureSubImage1D(Handle, level, offset, size, format, type, (nint)ptr);
        }
    }

    /// <summary>
    /// Sets the texture data at level 0 and generates the mipmaps.
    /// </summary>
    public void SetData<T>(ReadOnlySpan<T> data, int offset, int size, PixelFormat format, PixelType type) where T : unmanaged {
        SetData(data, 0, offset, size, format, type);
        GL.GenerateTextureMipmap(Handle);
    }
}