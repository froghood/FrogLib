

using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class BindlessTextureLibrary : Module {


    private FastStack<TextureHandle> handles = new();
    private ExpandingArray<Texture> textures = new();
    private Dictionary<string, int> indices = new();

    private Buffer buffer = new();


    public void Add(string name, Texture texture, bool makeResident = false) {

        if (indices.ContainsKey(name)) throw new ArgumentException($"Cannot add texture. Texture with the name {name} already present in the library.");

        var handle = texture.CreateTextureHandle();
        if (makeResident) handle.MakeResident();

        if (indices.TryGetValue(name, out var index)) {
            handles[index].MakeNonResident();
            textures[index].Dispose();
            handles[index] = handle;
        } else {
            index = handles.Push(handle);
            indices[name] = index;
        }

        textures[index] = texture;
    }

    public Texture GetTexture(string name) {
        if (indices.TryGetValue(name, out var index)) {
            return textures[index];
        }

        throw new ArgumentException($"Cannot get texture. No texture with the name {name} found in the library.");
    }

    public bool TryGetTexture(string name, [NotNullWhen(true)] out Texture? texture) {
        if (indices.TryGetValue(name, out var index)) {
            texture = textures[index];
            return true;
        }

        texture = null;
        return false;
    }

    public TextureHandle GetHandle(string name) {
        if (!indices.TryGetValue(name, out var index)) throw new ArgumentException($"Cannot get handle. No texture with the name {name} found in the library.");
        return handles[index];
    }

    public bool TryGetHandle(string name, out TextureHandle handle) {
        if (TryGetIndex(name, out var index)) {
            handle = handles[index];
            return true;
        } else {
            handle = default;
            return false;
        }
    }

    /// <summary>
    /// Returns the index of the texture in the library, -1 if not found.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetTextureIndex(string name) {
        if (indices.TryGetValue(name, out var index)) return index;
        return -1;
    }

    public bool TryGetIndex(string name, out int index) {
        return indices.TryGetValue(name, out index);
    }

    public void BufferHandles() {
        buffer.BufferData(handles.Span());
    }

    public void UseBuffer(BufferRangeTarget target, int index) {
        buffer.Use(target, index);
    }

    public ReadOnlySpan<TextureHandle> GetHandles() {
        return handles.Span();
    }

    public ReadOnlySpan<Texture> GetTextures() {
        return new ReadOnlySpan<Texture>(textures, 0, handles.Length);
    }

    public void LoadFile(string path, string name, int levels, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool makeResident = false) {
        var texture = Texture2d.FromFile(path, levels, preMultiply, verticalFlip);

        for (int i = 0; i < parameters.Length; i++) {
            texture.SetParam(parameters[i]);
        }

        Add(name, texture, makeResident);
    }

    public void LoadFiles(string dir, int levels, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool makeResident = false, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            LoadFile(path, name, levels, parameters, preMultiply, verticalFlip, makeResident);
        }
    }
}