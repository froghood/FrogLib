using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class RenderableCircle : DefaultRenderable {


    public float Radius { get; set; }
    public float OutlineWidth { get; set; } = 1f;

    public Color4 FillColor { get; set; } = Color4.White;
    public Color4 OutlineColor { get; set; } = Color4.Black;

    static VertexArray vertexArray;

    static RenderableCircle() {
        vertexArray = new VertexArray(
            new Layout(VertexAttribPointerType.Float, 2), // position
            new Layout(VertexAttribPointerType.Float, 2) // radius
        );

        vertexArray.BufferIndices(new int[]{
            0, 1, 2,
            1, 2, 3
        }, BufferUsageHint.StaticDraw);
    }

    public override void Render() {

        float diameter = Radius * 2f;

        float[] vertices = {
            0f, 0f, -Radius, -Radius,
            1f, 0f, Radius, -Radius,
            0f, 1f, -Radius, Radius,
            1f, 1f, Radius, Radius
        };

        var modelMatrix = Matrix4.Identity;
        modelMatrix *= Matrix4.CreateTranslation(-Origin.X, -Origin.Y, 0f);
        modelMatrix *= Matrix4.CreateRotationZ(Rotation);
        modelMatrix *= Matrix4.CreateScale(diameter * Scale.X, diameter * Scale.Y, 0f);
        modelMatrix *= Matrix4.CreateTranslation(Position.X, Position.Y, 0f);

        var projectionMatrix = Matrix4.CreateOrthographicOffCenter(
            0,
            Game.WindowSize.X,
            Game.WindowSize.Y,
            0,
            -1f, 1f);

        vertexArray.BufferVertexData(vertices, BufferUsageHint.DynamicDraw);
        vertexArray.Bind();

        var shaderLibrary = Game.Get<ShaderLibrary>();

        shaderLibrary.UseShader("circle");

        shaderLibrary.Uniform("modelMatrix", modelMatrix);
        shaderLibrary.Uniform("projectionMatrix", projectionMatrix);

        shaderLibrary.Uniform("radius", Radius);
        shaderLibrary.Uniform("outlineWidth", OutlineWidth);

        shaderLibrary.Uniform("scale", Scale);

        shaderLibrary.Uniform("fillColor", FillColor);
        shaderLibrary.Uniform("outlineColor", OutlineColor);

        Blend();

        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexCount, DrawElementsType.UnsignedInt, 0);
    }
}