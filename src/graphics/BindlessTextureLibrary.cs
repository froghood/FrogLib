

using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class BindlessTextureLibrary : GameSystem {


    private FastStack<TextureHandle> handles = new();
    private ExpandingArray<ITexture> textures = new();
    private Dictionary<string, int> indices = new();

    private Buffer buffer = new();


    public void Add(string name, ITexture texture) {


        if (indices.TryGetValue(name, out var index)) {
            throw new Exception($"Texture with the name {name} already present in the library.");
        }

        index = handles.Push(texture.CreateHandle());
        textures[index] = texture;

        indices[name] = index;
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

}