
namespace FrogLib.Mathematics;

public readonly partial struct Vec4i : IEquatable<Vec4i> {

    private readonly int x;
    private readonly int y;
    private readonly int z;
    private readonly int w;



    public int LengthSquared => x * x + y * y + z * z + w * w;
    public int LengthManhattan => Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(w);
    public float Length => MathF.Sqrt(LengthSquared);



    public static Vec4i Abs(Vec4i e) => new Vec4i(Math.Abs(e.x), Math.Abs(e.y), Math.Abs(e.z), Math.Abs(e.w));

    public static Vec4i Min(Vec4i a, Vec4i b) => new Vec4i(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z), Math.Min(a.w, b.w));

    public static Vec4i Max(Vec4i a, Vec4i b) => new Vec4i(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z), Math.Max(a.w, b.w));

    public static Vec4i Clamp(Vec4i e, Vec4i min, Vec4i max) {
        return new Vec4i(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y,
            e.z < min.z ? min.z : e.z > max.z ? max.z : e.z,
            e.w < min.w ? min.w : e.w > max.w ? max.w : e.w
        );
    }

    public static int Dot(Vec4i a, Vec4i b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

    public static int DistanceSquared(Vec4i a, Vec4i b) => (a - b).LengthSquared;

    public static int DistanceManhattan(Vec4i a, Vec4i b) => (a - b).LengthManhattan;



    public static Vec4i operator +(Vec4i a, Vec4i b) => new Vec4i(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

    public static Vec4i operator -(Vec4i a, Vec4i b) => new Vec4i(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

    public static Vec4i operator -(Vec4i a) => new Vec4i(-a.x, -a.y, -a.z, -a.W);

    public static Vec4i operator *(Vec4i a, Vec4i b) => new Vec4i(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);

    public static Vec4i operator *(Vec4i a, int b) => new Vec4i(a.x * b, a.y * b, a.z * b, a.w * b);

    public static Vec4i operator /(Vec4i a, Vec4i b) => new Vec4i(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);

    public static Vec4i operator /(Vec4i a, int b) => new Vec4i(a.x / b, a.y / b, a.z / b, a.w / b);

    public static bool operator ==(Vec4i left, Vec4i right) => left.Equals(right);

    public static bool operator !=(Vec4i left, Vec4i right) => !left.Equals(right);



    public bool Equals(Vec4i other) => x == other.x && y == other.y && z == other.z && w == other.w;
    public override bool Equals(object? obj) => obj is Vec4i other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(x, y, z, w);
    public override string ToString() => $"({x}, {y}, {z}, {w})";
}