
namespace FrogLib.Mathematics;

public readonly partial record struct Vec2i {

    private readonly int x;
    private readonly int y;



    public int LengthSquared => x * x + y * y;
    public int LengthManhattan => System.Math.Abs(x) + System.Math.Abs(y);
    public float Length => MathF.Sqrt(LengthSquared);
    public float Angle => MathF.Atan2(y, x);
    public Vec2i PerpendicularRight => new Vec2i(y, -x);
    public Vec2i PerpendicularLeft => new Vec2i(-y, x);



    public static Vec2i Abs(Vec2i e) => new Vec2i(System.Math.Abs(e.x), System.Math.Abs(e.y));

    public static Vec2i Min(Vec2i a, Vec2i b) => new Vec2i(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));

    public static Vec2i Max(Vec2i a, Vec2i b) => new Vec2i(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));

    public static Vec2i Clamp(Vec2i e, Vec2i min, Vec2i max) {
        return new Vec2i(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y
        );
    }

    public static int Dot(Vec2i a, Vec2i b) => a.x * b.x + a.y * b.y;

    public static int DistanceSquared(Vec2i a, Vec2i b) => (b - a).LengthSquared;

    public static int DistanceManhattan(Vec2i a, Vec2i b) => (b - a).LengthManhattan;



    public static Vec2i operator +(Vec2i a, Vec2i b) => new Vec2i(a.x + b.x, a.y + b.y);

    public static Vec2i operator -(Vec2i a, Vec2i b) => new Vec2i(a.x - b.x, a.y - b.y);

    public static Vec2i operator -(Vec2i a) => new Vec2i(-a.x, -a.y);

    public static Vec2i operator *(Vec2i a, Vec2i b) => new Vec2i(a.x * b.x, a.y * b.y);

    public static Vec2i operator *(Vec2i a, int b) => new Vec2i(a.x * b, a.y * b);

    public static Vec2i operator /(Vec2i a, Vec2i b) => new Vec2i(a.x / b.x, a.y / b.y);

    public static Vec2i operator /(Vec2i a, int b) => new Vec2i(a.x / b, a.y / b);
}