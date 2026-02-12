namespace FrogLib;

public class MeshInfo {

    public string Name => name;
    public int VertexCount => vertexCount;
    public int VertexSize => vertexSize;
    public int IndexCount => indexCount;
    public int FirstVertex => firstVertex;
    public int FirstIndex => firstIndex;
    public int VertexOffset => vertexOffset;
    public int IndexOffset => indexOffset;
    public int VertexDataSize => VertexCount * VertexSize;
    public int IndexDataSize => IndexCount * sizeof(int);

    private string name;
    private int vertexCount, vertexSize, indexCount, firstVertex, firstIndex, vertexOffset, indexOffset;



    public MeshInfo(string name, int vertexCount, int vertexSize, int indexCount, int firstVertex, int firstIndex, int vertexOffset, int indexOffset) {
        this.name = name;
        this.vertexCount = vertexCount;
        this.vertexSize = vertexSize;
        this.indexCount = indexCount;
        this.firstVertex = firstVertex;
        this.firstIndex = firstIndex;
        this.vertexOffset = vertexOffset;
        this.indexOffset = indexOffset;
    }

    public override string ToString() {
        return $"Name: {Name}, VertexCount: {VertexCount}, VertexSize: {VertexSize}, IndexCount: {IndexCount}, FirstVertex: {FirstVertex}, FirstIndex: {FirstIndex}, VertexOffset: {VertexOffset}, IndexOffset: {IndexOffset}, VertexDataSize: {VertexDataSize}, IndexDataSize: {IndexDataSize}";
    }
}
