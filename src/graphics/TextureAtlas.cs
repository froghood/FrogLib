using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace FrogLib;

public class TextureAtlas : IGameSystem {

    [JsonProperty]
    private int width;

    [JsonProperty]
    private int height;

    [JsonProperty]
    private Dictionary<string, SubTexture> sprites = new();

    internal TextureAtlas() { }


    public void Load(string dataPath) {
        var data = File.ReadAllText(dataPath);
        JsonConvert.PopulateObject(data, this);
    }

    public Box2 GetSpriteUV(string name) {



        var size = new Vector2(width, height);

        if (sprites.TryGetValue(name, out var bounds)) {

            return new Box2(new Vector2(bounds.Left, bounds.Top) / size, new Vector2(bounds.Right + 1f, bounds.Bottom + 1f) / size);

        } else {
            return default(Box2);
        }
    }

    public Box2 GetSpriteBounds(string name) {
        if (sprites.TryGetValue(name, out var bounds)) {
            return new Box2(new Vector2(bounds.Left, bounds.Top), new Vector2(bounds.Right + 1f, bounds.Bottom + 1f));
        } else {
            return default(Box2);
        }
    }

    public Vector2 GetSpriteSize(string name) {
        if (sprites.TryGetValue(name, out var bounds)) {
            return new Vector2(bounds.Right - bounds.Left + 1f, bounds.Bottom - bounds.Top + 1f);
        } else {
            return default(Vector2i);
        }
    }

    public Vector2 GetTextureSize() => new Vector2(width, height);
}

