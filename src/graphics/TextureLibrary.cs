using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class TextureLibrary : GameSystem {

    private Dictionary<string, ITexture> textures = new();

    public void Add(string name, ITexture texture) {
        if (textures.TryGetValue(name, out var prevTexture)) {
            prevTexture.Dispose();
        }

        textures[name] = texture;
    }

    public bool TryGet(string name, out ITexture? texture) {
        return textures.TryGetValue(name, out texture);
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

    public void LoadFile(string path, bool preMultiply = false, bool verticalFlip = true) {
        var texture = Texture2d.FromFile(path, preMultiply, verticalFlip);
        Add(Path.GetFileNameWithoutExtension(path), texture);
    }

    public void LoadFiles(string path, bool preMultiply = false, bool verticalFlip = true, bool recursive = false) {
        foreach (var file in Directory.EnumerateFiles(path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
            LoadFile(Path.GetRelativePath(path, file), preMultiply, verticalFlip);
        }
    }
}