
namespace FrogLib.Mathematics;

public readonly partial struct Vec3 : IEquatable<Vec3> {

    private readonly float x;
    private readonly float y;
    private readonly float z;



    public float LengthSquared => x * x + y * y + z * z;
    public float Length => MathF.Sqrt(LengthSquared);
    public Vec3 Normalized {
        get {
            var length = Length;
            if (length < float.Epsilon) return Zero;
            return this / length;
        }
    }



    public static Vec3 Abs(Vec3 e) => new Vec3(MathF.Abs(e.x), MathF.Abs(e.y), MathF.Abs(e.z));

    public static Vec3 Round(Vec3 e) => new Vec3(MathF.Round(e.x), MathF.Round(e.y), MathF.Round(e.z));

    public static Vec3 Floor(Vec3 e) => new Vec3(MathF.Floor(e.x), MathF.Floor(e.y), MathF.Floor(e.z));

    public static Vec3 Ceiling(Vec3 e) => new Vec3(MathF.Ceiling(e.x), MathF.Ceiling(e.y), MathF.Ceiling(e.z));

    public static Vec3 Min(Vec3 a, Vec3 b) => new Vec3(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y), MathF.Min(a.z, b.z));

    public static Vec3 Max(Vec3 a, Vec3 b) => new Vec3(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y), MathF.Max(a.z, b.z));

    public static Vec3 Clamp(Vec3 e, Vec3 min, Vec3 max) {
        return new Vec3(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y,
            e.z < min.z ? min.z : e.z > max.z ? max.z : e.z
        );
    }

    public static Vec3 Cross(Vec3 a, Vec3 b) => new Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);

    public static Vec3 Lerp(Vec3 a, Vec3 b, float t) => a + (b - a) * t;

    public static float Dot(Vec3 a, Vec3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

    public static float DistanceSquared(Vec3 a, Vec3 b) => (b - a).LengthSquared;

    public static float Distance(Vec3 a, Vec3 b) => (b - a).Length;



    public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);

    public static Vec3 operator -(Vec3 a) => new Vec3(-a.x, -a.y, -a.z);

    public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);

    public static Vec3 operator *(Vec3 a, float b) => new Vec3(a.x * b, a.y * b, a.z * b);

    public static Vec3 operator /(Vec3 a, Vec3 b) => new Vec3(a.x / b.x, a.y / b.y, a.z / b.z);

    public static Vec3 operator /(Vec3 a, float b) => new Vec3(a.x / b, a.y / b, a.z / b);

    public static bool operator ==(Vec3 left, Vec3 right) => left.Equals(right);

    public static bool operator !=(Vec3 left, Vec3 right) => !left.Equals(right);



    public bool Equals(Vec3 other) => x == other.x && y == other.y && z == other.z;
    public override bool Equals(object? obj) => obj is Vec3 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(x, y, z);
    public override string ToString() => $"({x}, {y}, {z})";
}