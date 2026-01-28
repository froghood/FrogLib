namespace FrogLib;

public struct MeshInfo {

    public string Name { get; init; }

    public int VertexCount { get; init; }
    public int VertexSize { get; init; }
    public int IndexCount { get; init; }

    public int FirstVertex { get; init; }
    public int FirstIndex { get; init; }

    public int VertexOffset { get; init; }
    public int IndexOffset { get; init; }

    public int VertexDataSize => VertexCount * VertexSize;
    public int IndexDataSize => IndexCount * sizeof(int);

    public override string ToString() {
        return $"Name: {Name}, VertexCount: {VertexCount}, VertexSize: {VertexSize}, IndexCount: {IndexCount}, FirstVertex: {FirstVertex}, FirstIndex: {FirstIndex}, VertexOffset: {VertexOffset}, IndexOffset: {IndexOffset}, VertexDataSize: {VertexDataSize}, IndexDataSize: {IndexDataSize}";
    }

}
