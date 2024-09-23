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
            new Layout(VertexAttribPointerType.Float, 2), // position
            new Layout(VertexAttribPointerType.Float, 2)  // uv
        );

        vertexArray.BufferIndices(new int[] { 0, 1, 2, 1, 2, 3 }, BufferUsageHint.StreamDraw);
    }
    public override void Render() {

        if (SpriteName is null) return;

        var spriteSize = Game.Graphics.SpriteAtlas.GetSpriteSize(SpriteName);
        var spriteBounds = Game.Graphics.SpriteAtlas.GetSpriteBounds(SpriteName);
        var textureSize = Game.Graphics.SpriteAtlas.GetTextureSize();

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
        vertexArray.Bind();

        Game.Graphics.ShaderLibrary.UseShader("pixelsprite");

        Game.Graphics.ShaderLibrary.Uniform("modelMatrix", modelMatrix);
        Game.Graphics.ShaderLibrary.Uniform("projectionMatrix", projectionMatrix);

        Game.Graphics.ShaderLibrary.Uniform("scale", Scale);
        Game.Graphics.ShaderLibrary.Uniform("spriteTopLeft", (Vector2)spriteBounds.Min);
        Game.Graphics.ShaderLibrary.Uniform("spriteBottomRight", (Vector2)spriteBounds.Max);
        Game.Graphics.ShaderLibrary.Uniform("textureSize", textureSize);
        Game.Graphics.ShaderLibrary.Uniform("color", Color);

        Game.Graphics.TextureLibrary.UseTexture("sprites", TextureUnit.Texture0);

        Blend();

        GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexCount, DrawElementsType.UnsignedInt, 0);
    }
}