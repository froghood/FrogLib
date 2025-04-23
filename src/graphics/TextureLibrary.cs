using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class TextureLibrary : GameSystem {

    private Dictionary<string, Texture> textures = new();



    public void Add(string name, Texture texture) {
        ThrowIfAlreadyPresent(name);
        textures.Add(name, texture);
    }

    public void Add(string name, string path, bool preMultiply = false, bool verticalFlip = true) {
        ThrowIfAlreadyPresent(name);
        textures.Add(name, new Texture(path, preMultiply, verticalFlip));
    }

    public void Add(string name, int width, int height, SizedInternalFormat format) {
        ThrowIfAlreadyPresent(name);
        textures.Add(name, new Texture(width, height, format));
    }

    public Texture Get(string name) {
        if (!textures.TryGetValue(name, out var texture)) return default;
        return texture;
    }

    public void Use(string name, int unit) {
        if (!textures.TryGetValue(name, out var texture)) return;
        texture.Use(unit);
    }

    public void UseImage(string name, int unit, TextureAccess access) {
        if (!textures.TryGetValue(name, out var texture)) return;
        texture.UseImage(unit, access);
    }

    public void SetParam(string name, TextureParameterName param, int value) {
        if (!textures.TryGetValue(name, out var texture)) return;
        texture.SetParam(param, value);
    }

    public void SetParam(string name, TextureParameterName param, float value) {
        if (!textures.TryGetValue(name, out var texture)) return;
        texture.SetParam(param, value);
    }

    private void ThrowIfAlreadyPresent(string name) {
        if (textures.ContainsKey(name)) throw new Exception($"Texture with the name {name} already present in the library.");
    }
}