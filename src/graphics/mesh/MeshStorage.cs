using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class MeshStorage : Module {


    int vao;
    private Buffer vertices = new();
    private Buffer indices = new();

    private FastStack<MeshInfo> meshInfo = new();
    private Dictionary<string, int> meshInfoIndices = new();


    public MeshStorage(int vertexBufferSize, int indexBufferSize) {

        vertices.BufferStorage(vertexBufferSize, BufferStorageFlags.DynamicStorageBit);

        GL.CreateVertexArrays(1, out vao);
        indices.BufferStorage(indexBufferSize, BufferStorageFlags.DynamicStorageBit);

        GL.VertexArrayElementBuffer(vao, indices.Id);
    }

    public void Load(int vertexCount, int vertexSize, ReadOnlySpan<byte> vertexData, ReadOnlySpan<uint> indexData, string name) {

        if (meshInfoIndices.ContainsKey(name)) {
            throw new ArgumentException($"Cannot load mesh; a mesh with the name \"{name}\" already stored.");
        }

        int vertexDataSize = vertexCount * vertexSize;

        if (vertexDataSize != vertexData.Length) {
            throw new ArgumentException($"Invalid vertex count or vertex size. Computed length: {vertexDataSize}, actual length: {vertexData.Length}");
        }

        MeshInfo? previousMesh = meshInfo.Length > 0 ? meshInfo[^1] : null;

        int firstVertex = (previousMesh?.FirstVertex + previousMesh?.VertexCount) ?? 0;
        int firstIndex = (previousMesh?.FirstIndex + previousMesh?.IndexCount) ?? 0;
        int vertexOffset = (previousMesh?.VertexOffset + previousMesh?.VertexDataSize) ?? 0;
        int indexOffset = (previousMesh?.IndexOffset + previousMesh?.IndexDataSize) ?? 0;

        vertices.BufferSubData(vertexData, vertexOffset);
        indices.BufferSubData(indexData, indexOffset);

        var mesh = new MeshInfo() {
            Name = name,

            VertexCount = vertexCount,
            VertexSize = vertexSize,
            IndexCount = indexData.Length,

            FirstVertex = firstVertex,
            FirstIndex = firstIndex,

            VertexOffset = vertexOffset,
            IndexOffset = indexOffset,
        };

        int index = meshInfo.Push(mesh);
        meshInfoIndices[name] = index;
    }

    public void Load(string path, string name) {

        if (meshInfoIndices.ContainsKey(name)) {
            throw new ArgumentException($"Cannot load mesh; a mesh with the name \"{name}\" already stored.");
        }

        byte[] data = File.ReadAllBytes(path);

        var deserializer = new FastDeserializer(data);

        deserializer.Read<byte>(out var magic, 4);
        if (Encoding.UTF8.GetString(magic) != "RVTX") return;

        deserializer.Read(out int vertexCount);
        deserializer.Read(out int attributeCount);

        int vertexSize = 0;
        for (int i = 0; i < attributeCount; i++) {
            deserializer.Read(out int size);
            vertexSize += size;
        }

        deserializer.Read(out int indexCount);

        deserializer.Read<byte>(out var vertexData, vertexCount * vertexSize);
        deserializer.Read<uint>(out var indexData, indexCount);

        Load(vertexCount, vertexSize, vertexData, indexData, name);

    }

    public void Unload(string name) {

        if (!meshInfoIndices.TryGetValue(name, out var index)) {
            throw new ArgumentException($"Cannot unload mesh; no mesh with the name \"{name}\" stored.");
        }

        var lastMesh = meshInfo[^1];

        var unloadedMesh = meshInfo.Remove(index);
        meshInfoIndices.Remove(name);

        if (meshInfo.Length == 0) return;

        int vertexDataCopyOffset = unloadedMesh.VertexOffset + unloadedMesh.VertexDataSize;
        int indexDataCopyOffset = unloadedMesh.IndexOffset + unloadedMesh.IndexDataSize;

        int vertexDataCopySize = lastMesh.VertexOffset + lastMesh.VertexDataSize - vertexDataCopyOffset;
        int indexDataCopySize = lastMesh.IndexOffset + lastMesh.IndexDataSize - indexDataCopyOffset;

        using var vertexDataCopy = new Buffer();
        using var indexDataCopy = new Buffer();

        vertexDataCopy.BufferStorage(vertexDataCopySize, BufferStorageFlags.DynamicStorageBit);
        indexDataCopy.BufferStorage(indexDataCopySize, BufferStorageFlags.DynamicStorageBit);

        vertices.CopyData(vertexDataCopy, vertexDataCopySize, vertexDataCopyOffset, 0);
        indices.CopyData(indexDataCopy, indexDataCopySize, indexDataCopyOffset, 0);

        vertexDataCopy.CopyData(vertices, vertexDataCopySize, 0, unloadedMesh.VertexOffset);
        indexDataCopy.CopyData(indices, indexDataCopySize, 0, unloadedMesh.IndexOffset);

        for (int i = index; i < meshInfo.Length; i++) {
            ref var mesh = ref meshInfo[i];

            meshInfoIndices[mesh.Name] = i;

            meshInfo[i] = new MeshInfo() {
                Name = mesh.Name,

                VertexCount = mesh.VertexCount,
                VertexSize = mesh.VertexSize,
                IndexCount = mesh.IndexCount,

                FirstVertex = mesh.FirstVertex - unloadedMesh.VertexCount,
                FirstIndex = mesh.FirstIndex - unloadedMesh.IndexCount,

                VertexOffset = mesh.VertexOffset - unloadedMesh.VertexDataSize,
                IndexOffset = mesh.IndexOffset - unloadedMesh.IndexDataSize,
            };
        }
    }


    public void UseBuffers(int unit) {
        vertices.Use(BufferRangeTarget.ShaderStorageBuffer, unit);
        GL.BindVertexArray(vao);
    }

    public void LoadFiles(string dir, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            Load(path, name);
        }
    }

    public MeshInfo GetMeshInfo(string name) {

        if (!meshInfoIndices.TryGetValue(name, out var index)) {
            throw new ArgumentException($"Cannot get mesh info; no mesh with the name \"{name}\" stored.");
        }

        return meshInfo[index];
    }

    public bool TryGetMeshInfo(string name, [NotNullWhen(true)] out MeshInfo? meshInfo) {

        if (meshInfoIndices.TryGetValue(name, out var index)) {
            meshInfo = this.meshInfo[index];
            return true;
        }

        meshInfo = null;
        return false;
    }

    protected internal override void Shutdown() {
        vertices.Dispose();
        indices.Dispose();
        GL.DeleteVertexArrays(1, ref vao);
    }

}