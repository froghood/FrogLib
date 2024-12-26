using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

/// <summary>
/// A pixel art sprite; uses a different shader for scaling pixel art smoothly
/// </summary>
public class PixelSprite : DefaultRenderable {

    public string? SpriteName { get; set; }
    public Color4 Color { get; set; } = Color4.White;

    private static VertexArray vertexArray;

    static PixelSprite() {
        vertexArray = new VertexArray(
            new Layout(VertexAttribType.Float, 2), // position
            new Layout(VertexAttribType.Float, 2)  // uv
        );

        vertexArray.BufferIndices(new int[] { 0, 1, 2, 1, 2, 3 }, BufferUsageHint.StreamDraw);
    }
    public override void Render() {

        if (SpriteName is null) return;

        var spriteAtlas = Game.Get<TextureAtlas>();

        var spriteSize = spriteAtlas.GetSpriteSize(SpriteName);
        var spriteBounds = spriteAtlas.GetSpriteBounds(SpriteName);
        var textureSize = spriteAtlas.GetTextureSize();

        float[] vertices = {
            0f, 0f, spriteBounds.Min.X - 1f, spriteBounds.Min.Y - 1f,
            1f, 0f, spriteBounds.Max.X + 1f, spriteBounds.Min.Y - 1f,
            0f, 1f, spriteBounds.Min.X - 1f, spriteBounds.Max.Y + 1f,
            1f, 1f, spriteBounds.Max.X + 1f, spriteBounds.Max.Y + 1f
        };

        var modelMatrix = Matrix4.Identity;
        modelMatrix *= Matrix4.CreateScale(spriteSize.X + 2f, spriteSize.Y + 2f, 0f);
        modelMatrix *= Matrix4.CreateTranslation(-Origin.X * spriteSize.X - 1f, -Origin.Y * spriteSize.Y - 1f, 0f);
        modelMatrix *= Matrix4.CreateScale(Scale.X, Scale.Y, 0f);
        modelMatrix *= Matrix4.CreateRotationZ(Rotation);
        modelMatrix *= Matrix4.CreateTranslation(Position.X, Position.Y, 0f);


        var projectionMatrix = Matrix4.CreateOrthographicOffCenter(
            0,
            Game.WindowSize.X,
            Game.WindowSize.Y,
            0,
            -1f, 1f);

        vertexArray.BufferVertexData(vertices, BufferUsageHint.DynamicDraw);
        vertexArray.Use();

        var shader = Game.Get<ShaderLibrary>().Get("pixelSprite");

        shader.Uniform("modelMatrix", modelMatrix);
        shader.Uniform("projectionMatrix", projectionMatrix);

        shader.Uniform("scale", Scale);
        shader.Uniform("spriteTopLeft", (Vector2)spriteBounds.Min);
        shader.Uniform("spriteBottomRight", (Vector2)spriteBounds.Max);
        shader.Uniform("textureSize", textureSize);
        shader.Uniform("color", Color);

        shader.Use();

        Game.Get<TextureLibrary>().Use("sprites", 0);

        Blend();

        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexCount, DrawElementsType.UnsignedInt, 0);
    }
}