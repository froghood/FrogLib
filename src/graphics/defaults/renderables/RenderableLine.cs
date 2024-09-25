using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class RenderableLine : DefaultRenderable {

    public Vector2 EndPosition { get; set; }

    public float Width { get; set; } = 1f;
    public Color4 Color { get; set; } = Color4.Black;

    private static VertexArray vertexArray;

    static RenderableLine() {
        vertexArray = new VertexArray(
            new Layout(VertexAttribPointerType.Float, 2) // position
        );

        vertexArray.BufferIndices(new int[] {
            0, 1, 2,
            1, 2, 3
        }, BufferUsageHint.StaticDraw);
    }

    public override void Render() {

        var size = EndPosition - Position;
        var direction = size / MathF.Max(size.Length, float.Epsilon) / 2f * Width;

        var tl = direction.PerpendicularLeft;
        var tr = size + direction.PerpendicularLeft;
        var bl = direction.PerpendicularRight;
        var br = size + direction.PerpendicularRight;

        float[] vertices = {
            tl.X, tl.Y,
            tr.X, tr.Y,
            bl.X, bl.Y,
            br.X, br.Y
        };

        var modelMatrix = Matrix4.Identity;
        modelMatrix *= Matrix4.CreateTranslation(-size.X * Origin.X, -size.Y * Origin.Y, 0f);
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

        shaderLibrary.UseShader("line");

        shaderLibrary.Uniform("modelMatrix", modelMatrix);
        shaderLibrary.Uniform("projectionMatrix", projectionMatrix);

        shaderLibrary.Uniform("color", Color);

        Blend();

        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexCount, DrawElementsType.UnsignedInt, 0);

    }


}