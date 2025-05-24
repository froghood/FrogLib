
namespace FrogLib.Mathematics;

public readonly partial struct Vec3 : IEquatable<Vec3> {

    public float X => x;
    public float Y => y;
    public float Z => z;

    public Vec2 XX => new Vec2(x, x);
    public Vec2 XY => new Vec2(x, y);
    public Vec2 XZ => new Vec2(x, z);
    public Vec2 YX => new Vec2(y, x);
    public Vec2 YY => new Vec2(y, y);
    public Vec2 YZ => new Vec2(y, z);
    public Vec2 ZX => new Vec2(z, x);
    public Vec2 ZY => new Vec2(z, y);
    public Vec2 ZZ => new Vec2(z, z);

    public Vec3 XXX => new Vec3(x, x, x);
    public Vec3 XXY => new Vec3(x, x, y);
    public Vec3 XXZ => new Vec3(x, x, z);
    public Vec3 XYX => new Vec3(x, y, x);
    public Vec3 XYY => new Vec3(x, y, y);
    public Vec3 XYZ => new Vec3(x, y, z);
    public Vec3 XZX => new Vec3(x, z, x);
    public Vec3 XZY => new Vec3(x, z, y);
    public Vec3 XZZ => new Vec3(x, z, z);
    public Vec3 YXX => new Vec3(y, x, x);
    public Vec3 YXY => new Vec3(y, x, y);
    public Vec3 YXZ => new Vec3(y, x, z);
    public Vec3 YYX => new Vec3(y, y, x);
    public Vec3 YYY => new Vec3(y, y, y);
    public Vec3 YYZ => new Vec3(y, y, z);
    public Vec3 YZX => new Vec3(y, z, x);
    public Vec3 YZY => new Vec3(y, z, y);
    public Vec3 YZZ => new Vec3(y, z, z);
    public Vec3 ZXX => new Vec3(z, x, x);
    public Vec3 ZXY => new Vec3(z, x, y);
    public Vec3 ZXZ => new Vec3(z, x, z);
    public Vec3 ZYX => new Vec3(z, y, x);
    public Vec3 ZYY => new Vec3(z, y, y);
    public Vec3 ZYZ => new Vec3(z, y, z);
    public Vec3 ZZX => new Vec3(z, z, x);
    public Vec3 ZZY => new Vec3(z, z, y);
    public Vec3 ZZZ => new Vec3(z, z, z);
}