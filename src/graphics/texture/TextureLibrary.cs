using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class TextureLibrary : Module {

    private Dictionary<string, Texture> textures = new();

    public void Add(string name, Texture texture) {
        if (textures.TryGetValue(name, out var prevTexture)) {
            prevTexture.Dispose();
        }

        textures[name] = texture;
    }

    public Texture GetTexture(string name) {
        if (!textures.TryGetValue(name, out var texture)) throw new ArgumentException($"No texture with the name {name} found in the library.");
        return texture;
    }

    public bool TryGetTexture(string name, [NotNullWhen(true)] out Texture? texture) {
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

    public void LoadFile(string path, string name, int levels, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true) {
        var texture = Texture2d.FromFile(path, levels, preMultiply, verticalFlip);

        for (int i = 0; i < parameters.Length; i++) {
            texture.SetParam(parameters[i]);
        }

        Add(name, texture);
    }

    public void LoadFiles(string dir, int levels, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            LoadFile(path, name, levels, parameters, preMultiply, verticalFlip);
        }
    }

    protected internal override void Shutdown() {
        foreach (var texture in textures.Values) texture.Dispose();
    }
}