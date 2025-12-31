using System.Runtime.CompilerServices;
using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public unsafe struct TextureParameter {

    public TextureParameterName Name => name;

    private readonly TextureParameterName name;
    private readonly TextureParamType type;
    private Byte16 data;

    public TextureParameter(TextureParameterName name, int value) {
        this.name = name;
        type = TextureParamType.Int;
        data = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, float value) {
        this.name = name;
        type = TextureParamType.Float;
        data = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, Vec2 value) {
        this.name = name;
        type = TextureParamType.Float;
        data = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Vec3 value) {
        this.name = name;
        type = TextureParamType.Float;
        data = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Vec4 value) {
        this.name = name;
        type = TextureParamType.Float;
        data = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Color4 value) {
        this.name = name;
        type = TextureParamType.Float;
        data = Byte16.Create(value);
    }

    public void Apply(int textureId) {

        if (textureId == 0) return;

        var ptr = Unsafe.AsPointer(ref data);

        switch (type) {
            case TextureParamType.Float: GL.TextureParameter(textureId, name, (float*)ptr); break;
            case TextureParamType.Int: GL.TextureParameter(textureId, name, (int*)ptr); break;
        }
    }

    public struct Byte16 {
        public fixed byte Data[16];

        public static Byte16 Create<T>(T value) where T : unmanaged {
            var data = new Byte16();
            Unsafe.Copy(&data.Data, ref value);
            return data;
        }
    }

    public enum TextureParamType {
        Float,
        Int
    }
}