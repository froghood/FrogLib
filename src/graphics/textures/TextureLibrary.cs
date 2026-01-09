using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class TextureLibrary : Module {

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

    public void SetParam(string name, in TextureParameter parameter) {
        if (!textures.TryGetValue(name, out var texture)) return;
        texture.SetParam(parameter);
    }

    public void LoadFile(string path, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true) {
        var texture = Texture2d.FromFile(path, preMultiply, verticalFlip);

        for (int i = 0; i < parameters.Length; i++) {
            texture.SetParam(parameters[i]);
        }

        Add(Path.GetFileNameWithoutExtension(path), texture);
    }

    public void LoadFiles(string path, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool recursive = false) {
        foreach (var file in Directory.EnumerateFiles(path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
            LoadFile(Path.GetRelativePath(path, file), parameters, preMultiply, verticalFlip);
        }
    }
}