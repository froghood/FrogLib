namespace FrogLib;

public struct DrawArraysIndirectCommand {
    public int Count { get; init; }
    public int InstanceCount { get; init; }
    public int FirstIndex { get; init; }
    public int BaseInstance { get; init; }

    public override string ToString() {
        return $"Count: {Count}, InstanceCount: {InstanceCount}, FirstIndex: {FirstIndex}, BaseInstance: {BaseInstance}";
    }
}

public struct DrawElementsIndirectCommand {
    public int Count { get; init; }
    public int InstanceCount { get; init; }
    public int FirstIndex { get; init; }
    public int BaseVertex { get; init; }
    public int BaseInstance { get; init; }

    public override string ToString() {
        return $"Count: {Count}, InstanceCount: {InstanceCount}, FirstIndex: {FirstIndex}, BaseVertex: {BaseVertex}, BaseInstance: {BaseInstance}";
    }
}