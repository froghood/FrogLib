using System.Diagnostics.CodeAnalysis;

namespace FrogLib;

public class TextureLibrary : ResourceLibrary<Texture> {

    public int Add(string name, Texture texture) => AddResource(name, texture);

    public void Remove(string name, bool dispose = true) {
        var texture = RemoveResource(name);

        if (dispose) texture.Dispose();
    }



    public void LoadFile(string path, string name, int levels, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true) {
        var texture = Texture2d.FromFile(path, levels, preMultiply, verticalFlip);

        for (int i = 0; i < parameters.Length; i++) {
            texture.SetParam(parameters[i]);
        }

        AddResource(name, texture);
    }

    public void LoadFiles(string dir, int levels, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            LoadFile(path, name, levels, parameters, preMultiply, verticalFlip);
        }
    }



    protected internal override void Shutdown() {
        foreach (var resource in GetResources()) {
            resource.Dispose();
        }
    }



    protected override string AlreadyPresentMessage(string name) => $"Texture with the name '{name}' is already present in the library.";
    protected override string NotFoundMessage(string name) => $"No texture with the name '{name}' found in the library.";
    protected override string NotFoundMessage(int id) => $"No texture with the id '{id}' found in the library.";
}