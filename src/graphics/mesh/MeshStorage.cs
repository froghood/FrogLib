using System.Diagnostics.CodeAnalysis;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class MeshStorage : ResourceLibrary<MeshInfo> {

    int vao;
    private Buffer vertices = new();
    private Buffer indices = new();

    private int nextVertex, nextIndex, nextVertexOffset, nextIndexOffset;



    public MeshStorage(int vertexBufferSize, int indexBufferSize) {

        vertices.BufferStorage(vertexBufferSize, BufferStorageFlags.DynamicStorageBit);

        GL.CreateVertexArrays(1, out vao);
        indices.BufferStorage(indexBufferSize, BufferStorageFlags.DynamicStorageBit);

        GL.VertexArrayElementBuffer(vao, indices.Handle);
    }



    public int Load(int vertexCount, int vertexSize, ReadOnlySpan<byte> vertexData, ReadOnlySpan<uint> indexData, string name) {

        int vertexDataSize = vertexCount * vertexSize;

        if (vertexDataSize != vertexData.Length) {
            throw new ArgumentException($"Invalid vertex count or vertex size. Computed length: {vertexDataSize}, actual length: {vertexData.Length}");
        }

        vertices.BufferSubData(vertexData, nextVertexOffset);
        indices.BufferSubData(indexData, nextIndexOffset);

        var mesh = new MeshInfo(
            name,
            vertexCount,
            vertexSize,
            indexData.Length,
            nextVertex,
            nextIndex,
            nextVertexOffset,
            nextIndexOffset
        );

        nextVertex += vertexCount;
        nextIndex += indexData.Length;
        nextVertexOffset += vertexDataSize;
        nextIndexOffset += indexData.Length * sizeof(uint);

        return AddResource(name, mesh);
    }

    public int Load(string path, string name) {

        byte[] data = File.ReadAllBytes(path);

        var deserializer = new FastDeserializer(data);

        deserializer.Read<byte>(out var magic, 4);
        if (Encoding.UTF8.GetString(magic) != "RVTX") return -1;

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

        return Load(vertexCount, vertexSize, vertexData, indexData, name);
    }

    public void LoadFiles(string dir, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            Load(path, name);
        }
    }



    public void Unload(string name) {

        var mesh = RemoveResource(name, (mesh, other) => {
            return new MeshInfo(
                other.Name,
                other.VertexCount,
                other.VertexSize,
                other.IndexCount,
                other.FirstVertex - mesh.VertexCount,
                other.FirstIndex - mesh.IndexCount,
                other.VertexOffset - mesh.VertexDataSize,
                other.IndexOffset - mesh.IndexDataSize
            );
        });

        int vertexDataCopyOffset = mesh.VertexOffset + mesh.VertexDataSize;
        int indexDataCopyOffset = mesh.IndexOffset + mesh.IndexDataSize;

        int vertexDataCopySize = nextVertexOffset - vertexDataCopyOffset;
        int indexDataCopySize = nextIndexOffset - indexDataCopyOffset;

        using var vertexDataCopy = new Buffer();
        using var indexDataCopy = new Buffer();

        vertexDataCopy.BufferStorage(vertexDataCopySize, BufferStorageFlags.DynamicStorageBit);
        indexDataCopy.BufferStorage(indexDataCopySize, BufferStorageFlags.DynamicStorageBit);

        vertices.CopyData(vertexDataCopy, vertexDataCopySize, vertexDataCopyOffset, 0);
        indices.CopyData(indexDataCopy, indexDataCopySize, indexDataCopyOffset, 0);

        vertexDataCopy.CopyData(vertices, vertexDataCopySize, 0, mesh.VertexOffset);
        indexDataCopy.CopyData(indices, indexDataCopySize, 0, mesh.IndexOffset);

        nextVertex -= mesh.VertexCount;
        nextIndex -= mesh.IndexCount;
        nextVertexOffset -= mesh.VertexDataSize;
        nextIndexOffset -= mesh.IndexDataSize;
    }



    public void UseBuffers(BufferRangeTarget target, int unit) {
        vertices.Use(target, unit);
        GL.BindVertexArray(vao);
    }



    protected internal override void Shutdown() {
        vertices.Dispose();
        indices.Dispose();
        GL.DeleteVertexArrays(1, ref vao);
    }



    protected override string AlreadyPresentMessage(string name) => $"Mesh with the name '{name}' is already present in the library.";
    protected override string NotFoundMessage(string name) => $"No mesh with the name '{name}' found in the library.";
    protected override string NotFoundMessage(int id) => $"No mesh with the id '{id}' found in the library.";
}