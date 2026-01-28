
namespace FrogLib.Mathematics;

public readonly partial struct Vec2i : IEquatable<Vec2i> {

    private readonly int x;
    private readonly int y;



    public int LengthSquared => x * x + y * y;
    public int LengthManhattan => Math.Abs(x) + Math.Abs(y);
    public float Length => MathF.Sqrt(LengthSquared);
    public float Angle => MathF.Atan2(y, x);

    public Vec2i PerpendicularRight => new Vec2i(y, -x);
    public Vec2i PerpendicularLeft => new Vec2i(-y, x);

    public int MinComponent => Math.Min(x, y);
    public int MaxComponent => Math.Max(x, y);



    public static Vec2i Abs(Vec2i e) => new Vec2i(Math.Abs(e.x), Math.Abs(e.y));

    public static Vec2i Min(Vec2i a, Vec2i b) => new Vec2i(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

    public static Vec2i Max(Vec2i a, Vec2i b) => new Vec2i(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

    public static Vec2i Clamp(Vec2i e, Vec2i min, Vec2i max) {
        return new Vec2i(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y
        );
    }

    public static int Dot(Vec2i a, Vec2i b) => a.x * b.x + a.y * b.y;

    public static int Cross(Vec2i a, Vec2i b) => a.x * b.y - a.y * b.x;

    public static int DistanceSquared(Vec2i a, Vec2i b) => (b - a).LengthSquared;

    public static int DistanceManhattan(Vec2i a, Vec2i b) => (b - a).LengthManhattan;



    public static Vec2i operator +(Vec2i a, Vec2i b) => new Vec2i(a.x + b.x, a.y + b.y);

    public static Vec2i operator -(Vec2i a, Vec2i b) => new Vec2i(a.x - b.x, a.y - b.y);

    public static Vec2i operator -(Vec2i a) => new Vec2i(-a.x, -a.y);

    public static Vec2i operator *(Vec2i a, Vec2i b) => new Vec2i(a.x * b.x, a.y * b.y);

    public static Vec2i operator *(Vec2i a, int b) => new Vec2i(a.x * b, a.y * b);

    public static Vec2i operator /(Vec2i a, Vec2i b) => new Vec2i(a.x / b.x, a.y / b.y);

    public static Vec2i operator /(Vec2i a, int b) => new Vec2i(a.x / b, a.y / b);

    public static bool operator ==(Vec2i left, Vec2i right) => left.Equals(right);

    public static bool operator !=(Vec2i left, Vec2i right) => !left.Equals(right);



    public bool Equals(Vec2i other) => x == other.x && y == other.y;
    public override bool Equals(object? obj) => obj is Vec2i other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(x, y);
    public override string ToString() => $"({x}, {y})";
}