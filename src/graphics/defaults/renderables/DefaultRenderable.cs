using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public abstract class DefaultRenderable : IRenderable {

    public float Depth { get; set; } = 0f;
    public Vector2 Origin { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public BlendMode BlendMode { get; set; } = BlendMode.Normal;

    public abstract void Render();

    protected void Blend() {
        switch (BlendMode) {
            case BlendMode.Normal:
                GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
                break;
            case BlendMode.Additive:
                GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                break;
            case BlendMode.Multiply:
                GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.OneMinusSrcAlpha);
                break;
        }
    }
}