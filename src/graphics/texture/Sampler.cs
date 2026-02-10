using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class Sampler : GLObject {
    public Sampler(ReadOnlySpan<SamplerParameter> parameters)
    : base(GL.CreateSampler()) {

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Handle);
        }
    }

    public void Use(int unit) {
        ThrowIfInvalid();
        GL.BindSampler(unit, Handle);
    }

    protected override void Delete() {
        GL.DeleteSampler(Handle);
    }

    public override int GetHashCode() => Handle;
}