
namespace FrogLib.Mathematics;

public readonly partial struct Vec3i : IEquatable<Vec3i> {

    public int X => x;
    public int Y => y;
    public int Z => z;

    public Vec2i XX => new Vec2i(x, x);
    public Vec2i XY => new Vec2i(x, y);
    public Vec2i XZ => new Vec2i(x, z);
    public Vec2i YX => new Vec2i(y, x);
    public Vec2i YY => new Vec2i(y, y);
    public Vec2i YZ => new Vec2i(y, z);
    public Vec2i ZX => new Vec2i(z, x);
    public Vec2i ZY => new Vec2i(z, y);
    public Vec2i ZZ => new Vec2i(z, z);

    public Vec3i XXX => new Vec3i(x, x, x);
    public Vec3i XXY => new Vec3i(x, x, y);
    public Vec3i XXZ => new Vec3i(x, x, z);
    public Vec3i XYX => new Vec3i(x, y, x);
    public Vec3i XYY => new Vec3i(x, y, y);
    public Vec3i XYZ => new Vec3i(x, y, z);
    public Vec3i XZX => new Vec3i(x, z, x);
    public Vec3i XZY => new Vec3i(x, z, y);
    public Vec3i XZZ => new Vec3i(x, z, z);
    public Vec3i YXX => new Vec3i(y, x, x);
    public Vec3i YXY => new Vec3i(y, x, y);
    public Vec3i YXZ => new Vec3i(y, x, z);
    public Vec3i YYX => new Vec3i(y, y, x);
    public Vec3i YYY => new Vec3i(y, y, y);
    public Vec3i YYZ => new Vec3i(y, y, z);
    public Vec3i YZX => new Vec3i(y, z, x);
    public Vec3i YZY => new Vec3i(y, z, y);
    public Vec3i YZZ => new Vec3i(y, z, z);
    public Vec3i ZXX => new Vec3i(z, x, x);
    public Vec3i ZXY => new Vec3i(z, x, y);
    public Vec3i ZXZ => new Vec3i(z, x, z);
    public Vec3i ZYX => new Vec3i(z, y, x);
    public Vec3i ZYY => new Vec3i(z, y, y);
    public Vec3i ZYZ => new Vec3i(z, y, z);
    public Vec3i ZZX => new Vec3i(z, z, x);
    public Vec3i ZZY => new Vec3i(z, z, y);
    public Vec3i ZZZ => new Vec3i(z, z, z);
}