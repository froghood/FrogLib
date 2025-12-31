

using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class BindlessTextureLibrary : GameSystem {


    private FastStack<TextureHandle> handles = new();
    private ExpandingArray<ITexture> textures = new();
    private Dictionary<string, int> indices = new();

    private Buffer buffer = new();


    public void Add(string name, ITexture texture, bool makeResident = false) {

        var handle = texture.CreateHandle();
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



    public bool TryGetTexture(string name, out ITexture texture) {
        if (TryGetIndex(name, out var index)) {
            texture = textures[index];
            return true;
        } else {
            texture = default!;
            return false;
        }
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

    public ReadOnlySpan<ITexture> GetTextures() {
        return new ReadOnlySpan<ITexture>(textures, 0, handles.Length);
    }

    public void LoadFile(string path, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool makeResident = false) {
        var texture = Texture2d.FromFile(path, preMultiply, verticalFlip);

        for (int i = 0; i < parameters.Length; i++) {
            texture.SetParam(parameters[i]);
        }

        Add(Path.GetFileNameWithoutExtension(path), texture, makeResident);
    }

    public void LoadFiles(string path, ReadOnlySpan<TextureParameter> parameters, bool preMultiply = false, bool verticalFlip = true, bool makeResident = false, bool recursive = false) {
        foreach (var file in Directory.EnumerateFiles(path, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {
            LoadFile(Path.GetRelativePath(path, file), parameters, preMultiply, verticalFlip, makeResident);
        }
    }

}