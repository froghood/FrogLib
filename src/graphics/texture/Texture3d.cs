using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class Texture3d : Texture {

    public Vec3i Size => size;

    private Vec3i size;

    public Texture3d(TextureTarget3d target, int levels, Vec3i size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(
        (TextureTarget)target,
        levels,
        size,
        format,
        parameters
    ) {

        this.size = size;
    }

    public unsafe void SetData<T>(ReadOnlySpan<T> data, int level, Box3i subregion, PixelFormat format, PixelType type) where T : unmanaged {

        ThrowIfInvalid();

        if (level < 0 || level >= Levels) throw new ArgumentException($"Mipmap Level {level} is outside the texture's available mipmap range: (0 - {Levels}).");

        var bounds = new Box3i(Vec3i.Zero, size);
        if (!bounds.FullyContains(subregion)) throw new ArgumentException("Attempted to set data outside the texture's bounds.");

        fixed (T* ptr = data) {
            GL.TextureSubImage3D(Handle, level, subregion.Min.X, subregion.Min.Y, subregion.Min.Z, subregion.Size.X, subregion.Size.Y, subregion.Size.Z, format, type, (nint)ptr);
        }
    }

    /// <summary>
    /// Sets the texture data at level 0 and generates the mipmaps.
    /// </summary>
    public void SetData<T>(ReadOnlySpan<T> data, Box3i subregion, PixelFormat format, PixelType type) where T : unmanaged {
        SetData(data, 0, subregion, format, type);
        GL.GenerateTextureMipmap(Handle);
    }
}

