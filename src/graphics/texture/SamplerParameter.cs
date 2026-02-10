using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public readonly struct SamplerParameter {

    public SamplerParameterName Name => name;

    private readonly SamplerParameterName name;
    private readonly Type type;
    private readonly Byte16 value;



    public SamplerParameter(SamplerParameterName name, bool value) {
        this.name = name;
        type = Type.Int;
        this.value = Byte16.Create(value ? 1 : 0);
    }

    public SamplerParameter(SamplerParameterName name, float value) {
        this.name = name;
        type = Type.Float;
        this.value = Byte16.Create(value);
    }

    public SamplerParameter(SamplerParameterName name, Color4 value) {
        this.name = name;
        type = Type.Float;
        this.value = Byte16.Create(value);
    }



    internal unsafe void Apply(int samplerId) {

        if (samplerId == 0) return;

        fixed (void* ptr = &value) {
            switch (type) {
                case Type.Float: GL.SamplerParameter(samplerId, name, (float*)ptr); break;
                case Type.Int: GL.SamplerParameter(samplerId, name, (int*)ptr); break;
            }
        }
    }



    private enum Type : byte {
        Float,
        Int
    }

    private unsafe struct Byte16 {
        private fixed byte bytes[16];
        public static Byte16 Create<T>(T value) where T : unmanaged => *(Byte16*)&value;
    }
}