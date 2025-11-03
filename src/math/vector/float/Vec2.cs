
using System.Diagnostics.CodeAnalysis;

namespace FrogLib.Mathematics;

public readonly partial struct Vec2 : IEquatable<Vec2> {

    private readonly float x;
    private readonly float y;


    public float LengthSquared => x * x + y * y;
    public float Length => MathF.Sqrt(LengthSquared);
    public float Angle => MathF.Atan2(y, x);
    public Vec2 Normalized {
        get {
            var length = Length;
            if (length < float.Epsilon) return Zero;
            return this / length;
        }
    }

    public Vec2 PerpendicularRight => new Vec2(y, -x);
    public Vec2 PerpendicularLeft => new Vec2(-y, x);



    public static Vec2 Abs(Vec2 e) => new Vec2(MathF.Abs(e.x), MathF.Abs(e.y));

    public static Vec2 Round(Vec2 e) => new Vec2(MathF.Round(e.x), MathF.Round(e.y));

    public static Vec2 Floor(Vec2 e) => new Vec2(MathF.Floor(e.x), MathF.Floor(e.y));

    public static Vec2 Ceiling(Vec2 e) => new Vec2(MathF.Ceiling(e.x), MathF.Ceiling(e.y));

    public static Vec2 Min(Vec2 a, Vec2 b) => new Vec2(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y));

    public static Vec2 Max(Vec2 a, Vec2 b) => new Vec2(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y));

    public static Vec2 Clamp(Vec2 e, Vec2 min, Vec2 max) {
        return new Vec2(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y
        );
    }


    public static Vec2 Lerp(Vec2 a, Vec2 b, float t) => a + (b - a) * t;

    public static float Dot(Vec2 a, Vec2 b) => a.x * b.x + a.y * b.y;

    public static float Cross(Vec2 a, Vec2 b) => a.x * b.y - a.y * b.x;

    public static float DistanceSquared(Vec2 a, Vec2 b) => (b - a).LengthSquared;

    public static float Distance(Vec2 a, Vec2 b) => (b - a).Length;



    public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.x + b.x, a.y + b.y);

    public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.x - b.x, a.y - b.y);

    public static Vec2 operator -(Vec2 a) => new Vec2(-a.x, -a.y);

    public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.x * b.x, a.y * b.y);

    public static Vec2 operator *(Vec2 a, float b) => new Vec2(a.x * b, a.y * b);

    public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.x / b.x, a.y / b.y);

    public static Vec2 operator /(Vec2 a, float b) => new Vec2(a.x / b, a.y / b);

    public static bool operator ==(Vec2 left, Vec2 right) => left.Equals(right);

    public static bool operator !=(Vec2 left, Vec2 right) => !left.Equals(right);



    public bool Equals(Vec2 other) => x == other.x && y == other.y;
    public override bool Equals(object? obj) => obj is Vec2 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(x, y);
    public override string ToString() => $"({x}, {y})";
}