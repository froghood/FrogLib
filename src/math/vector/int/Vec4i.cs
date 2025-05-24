
namespace FrogLib.Mathematics;

public readonly partial record struct Vec4i {

    private readonly int x;
    private readonly int y;
    private readonly int z;
    private readonly int w;



    public int LengthSquared => x * x + y * y + z * z + w * w;
    public int LengthManhattan => System.Math.Abs(x) + System.Math.Abs(y) + System.Math.Abs(z) + System.Math.Abs(w);
    public float Length => MathF.Sqrt(LengthSquared);



    public static Vec4i Abs(Vec4i e) => new Vec4i(System.Math.Abs(e.x), System.Math.Abs(e.y), System.Math.Abs(e.z), System.Math.Abs(e.w));

    public static Vec4i Min(Vec4i a, Vec4i b) => new Vec4i(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y), System.Math.Min(a.z, b.z), System.Math.Min(a.w, b.w));

    public static Vec4i Max(Vec4i a, Vec4i b) => new Vec4i(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y), System.Math.Max(a.z, b.z), System.Math.Max(a.w, b.w));

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
}