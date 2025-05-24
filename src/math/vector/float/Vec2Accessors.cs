
namespace FrogLib.Mathematics;

public readonly partial record struct Vec2 {
    public float X => x;
    public float Y => y;

    public Vec2 XX => new Vec2(x, x);
    public Vec2 XY => new Vec2(x, y);
    public Vec2 YY => new Vec2(y, y);
    public Vec2 YX => new Vec2(y, x);
}