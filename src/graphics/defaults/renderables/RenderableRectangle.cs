using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class RenderableRectangle : DefaultRenderable {

    public Vector2 Size { get; set; }

    public Color4 FillColor { get; set; } = Color4.White;
    public Color4 StrokeColor { get; set; } = Color4.Black;
    public float StrokeWidth { get; set; } = 1f;
    public StrokeMode StrokeMode { get; set; }

    private static VertexArray vertexArray;

    static RenderableRectangle() {
        vertexArray = new VertexArray(
            new Layout(VertexAttribPointerType.Float, 2) // position
        );

        vertexArray.BufferIndices(new int[] {
            0, 1, 4, // outer
            1, 4, 5,
            0, 2, 4,
            2, 4, 6,
            3, 1, 7,
            1, 7, 5,
            3, 2, 7,
            2, 7, 6,
            4, 5, 6, // inner
            5, 6, 7,
        }, BufferUsageHint.StaticDraw);
    }
    public override void Render() {

        float o = 1f - (float)StrokeMode / 2f;
        float i = (float)StrokeMode / 2f;
        var size = Size * Scale;

        float[] vertices = {
            
            // outer
            -StrokeWidth * o, -StrokeWidth * o,
            size.X + StrokeWidth * o, -StrokeWidth * o,
            -StrokeWidth * o, size.Y + StrokeWidth * o,
            size.X + StrokeWidth * o, size.Y + StrokeWidth * o,
            
            //inner
            StrokeWidth * i, StrokeWidth * i,
            size.X - StrokeWidth * i, StrokeWidth * i,
            StrokeWidth * i, size.Y - StrokeWidth * i,
            size.X - StrokeWidth * i, size.Y - StrokeWidth * i
        };

        var modelMatrix = Matrix4.Identity;

        modelMatrix *= Matrix4.CreateTranslation(-Size.X * Origin.X, -Size.Y * Origin.Y, 0f);
        modelMatrix *= Matrix4.CreateRotationZ(Rotation);
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

        shaderLibrary.UseShader("rectangle");

        shaderLibrary.Uniform("modelMatrix", modelMatrix);
        shaderLibrary.Uniform("projectionMatrix", projectionMatrix);

        shaderLibrary.Uniform("fillColor", FillColor);
        shaderLibrary.Uniform("strokeColor", StrokeColor);

        Blend();

        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexCount, DrawElementsType.UnsignedInt, 0);
    }
}