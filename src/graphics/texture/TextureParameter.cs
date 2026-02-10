using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public readonly struct TextureParameter {

    public TextureParameterName Name => name;

    private readonly TextureParameterName name;
    private readonly Type type;
    private readonly Byte16 value;



    public TextureParameter(TextureParameterName name, int value) {
        this.name = name;
        type = Type.Int;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, float value) {
        this.name = name;
        type = Type.Float;
        this.value = Byte16.Create(value);
    }

    public TextureParameter(TextureParameterName name, in Color4 value) {
        this.name = name;
        type = Type.Float;
        this.value = Byte16.Create(value);
    }



    internal unsafe void Apply(int textureId) {

        if (textureId == 0) return;

        fixed (void* ptr = &value) {
            switch (type) {
                case Type.Float: GL.TextureParameter(textureId, name, (float*)ptr); break;
                case Type.Int: GL.TextureParameter(textureId, name, (int*)ptr); break;
            }
        }
    }



    private enum Type {
        Float,
        Int
    }

    private unsafe struct Byte16 {
        private fixed byte Data[16];
        public static Byte16 Create<T>(T value) where T : unmanaged => *(Byte16*)&value;
    }
}