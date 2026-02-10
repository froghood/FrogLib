using System.Diagnostics.CodeAnalysis;

namespace FrogLib;

public class TextureLibrary : Module {

    private FastStack<Texture> textures = new();
    private Dictionary<int, int> indicesById = new();
    private Dictionary<string, int> idsByName = new();
    private int nextId = 0;

    public void Add(string name, Texture texture) {
        if (idsByName.ContainsKey(name)) throw new ArgumentException($"Texture with the name {name} already present in the library.");

        int id = nextId++;
        indicesById[id] = textures.Push(texture);
        idsByName[name] = id;
    }

    public Texture GetTexture(string name) {
        if (!idsByName.TryGetValue(name, out int id)) throw new ArgumentException($"No texture with the name {name} found in the library.");

        return textures[indicesById[id]];
    }

    public Texture GetTexture(int id) {

        if (!indicesById.TryGetValue(id, out int index)) throw new ArgumentException($"No texture with the id {id} found in the library.");

        return textures[index];
    }

    public int GetId(string name) {
        if (!idsByName.TryGetValue(name, out int id)) throw new ArgumentException($"No texture with the name {name} found in the library.");
        return id;
    }

    public bool TryGetTexture(string name, [NotNullWhen(true)] out Texture? texture) {
        if (!idsByName.TryGetValue(name, out int id)) {
            texture = null;
            return false;
        }

        texture = textures[indicesById[id]];
        return true;
    }

    public bool TryGetTexture(int id, [NotNullWhen(true)] out Texture? texture) {
        if (!indicesById.TryGetValue(id, out int index)) {
            texture = null;
            return false;
        }

        texture = textures[index];
        return true;
    }

    public bool TryGetId(string name, out int id) => idsByName.TryGetValue(name, out id);



    public void Remove(string name, bool dispose = true) {
        if (!idsByName.TryGetValue(name, out int id)) throw new ArgumentException($"No texture with the name {name} found in the library.");

        var texture = textures.Remove(indicesById[id]);
        if (dispose) texture.Dispose();

        indicesById.Remove(id);
        idsByName.Remove(name);
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
        for (int i = 0; i < textures.Length; i++) {
            textures[i].Dispose();
        }
    }
}