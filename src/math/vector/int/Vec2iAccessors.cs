
namespace FrogLib.Mathematics;

public readonly partial record struct Vec2i {
    public int X => x;
    public int Y => y;

    public Vec2i XX => new Vec2i(x, x);
    public Vec2i XY => new Vec2i(x, y);
    public Vec2i YY => new Vec2i(y, y);
    public Vec2i YX => new Vec2i(y, x);
}