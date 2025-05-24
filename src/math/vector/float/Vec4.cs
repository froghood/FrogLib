
namespace FrogLib.Mathematics;

public readonly partial record struct Vec4 {

    private readonly float x;
    private readonly float y;
    private readonly float z;
    private readonly float w;



    public float LengthSquared => x * x + y * y + z * z + w * w;
    public float Length => MathF.Sqrt(LengthSquared);



    public static Vec4 Abs(Vec4 e) => new Vec4(MathF.Abs(e.x), MathF.Abs(e.y), MathF.Abs(e.z), MathF.Abs(e.w));

    public static Vec4 Round(Vec4 e) => new Vec4(MathF.Round(e.x), MathF.Round(e.y), MathF.Round(e.z), MathF.Round(e.w));

    public static Vec4 Floor(Vec4 e) => new Vec4(MathF.Floor(e.x), MathF.Floor(e.y), MathF.Floor(e.z), MathF.Floor(e.w));

    public static Vec4 Ceiling(Vec4 e) => new Vec4(MathF.Ceiling(e.x), MathF.Ceiling(e.y), MathF.Ceiling(e.z), MathF.Ceiling(e.w));

    public static Vec4 Min(Vec4 a, Vec4 b) => new Vec4(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y), MathF.Min(a.z, b.z), MathF.Min(a.w, b.w));

    public static Vec4 Max(Vec4 a, Vec4 b) => new Vec4(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y), MathF.Max(a.z, b.z), MathF.Max(a.w, b.w));

    public static Vec4 Clamp(Vec4 e, Vec4 min, Vec4 max) {
        return new Vec4(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y,
            e.z < min.z ? min.z : e.z > max.z ? max.z : e.z,
            e.w < min.w ? min.w : e.w > max.w ? max.w : e.w
        );
    }

    public static Vec4 Lerp(Vec4 a, Vec4 b, float t) => a + (b - a) * t;

    public static float Dot(Vec4 a, Vec4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

    public static float DistanceSquared(Vec4 a, Vec4 b) => (b - a).LengthSquared;

    public static float Distance(Vec4 a, Vec4 b) => (b - a).Length;



    public Vec4 Normalized {
        get {
            var length = Length;
            if (length < float.Epsilon) return Zero;
            float scale = 1.0f / length;
            return this * scale;
        }
    }



    public static Vec4 operator +(Vec4 a, Vec4 b) => new Vec4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

    public static Vec4 operator -(Vec4 a, Vec4 b) => new Vec4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

    public static Vec4 operator -(Vec4 a) => new Vec4(-a.x, -a.y, -a.z, -a.W);

    public static Vec4 operator *(Vec4 a, Vec4 b) => new Vec4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);

    public static Vec4 operator *(Vec4 a, float b) => new Vec4(a.x * b, a.y * b, a.z * b, a.w * b);

    public static Vec4 operator /(Vec4 a, Vec4 b) => new Vec4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);

    public static Vec4 operator /(Vec4 a, float b) => new Vec4(a.x / b, a.y / b, a.z / b, a.w / b);
}