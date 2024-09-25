
namespace FrogLib;

public struct SubTexture {
    public int Left { get; init; }
    public int Top { get; init; }
    public int Right { get; init; }
    public int Bottom { get; init; }
    public bool IsRotated { get; init; }

    public override string ToString() {
        return $"{Left}, {Top} | {Right}, {Bottom}";
    }
}