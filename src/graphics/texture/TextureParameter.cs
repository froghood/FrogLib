using System.Runtime.CompilerServices;
using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public unsafe struct TextureParameter {

    public TextureParameterName Name => name;

    private readonly TextureParameterName name;
    private readonly TextureParamType type;
    private readonly Byte16 value;



    public TextureParameter(TextureParameterName name, int value) {
        this.name = name;
        type = TextureParamType.Int;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, float value) {
        this.name = name;
        type = TextureParamType.Float;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, Vec2 value) {
        this.name = name;
        type = TextureParamType.Float;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Vec3 value) {
        this.name = name;
        type = TextureParamType.Float;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Vec4 value) {
        this.name = name;
        type = TextureParamType.Float;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Color4 value) {
        this.name = name;
        type = TextureParamType.Float;
        this.value = Byte16.Create(value);
    }



    internal void Apply(int textureId) {

        if (textureId == 0) return;

        fixed (void* ptr = &value) {
            switch (type) {
                case TextureParamType.Float: GL.TextureParameter(textureId, name, (float*)ptr); break;
                case TextureParamType.Int: GL.TextureParameter(textureId, name, (int*)ptr); break;
            }
        }


    }



    private unsafe struct Byte16 {
        private fixed byte Data[16];
        public static Byte16 Create<T>(T value) where T : unmanaged => *(Byte16*)&value;
    }

    public enum TextureParamType {
        Float,
        Int
    }
}