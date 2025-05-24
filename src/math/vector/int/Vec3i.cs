
namespace FrogLib.Mathematics;

public readonly partial record struct Vec3i {

    private readonly int x;
    private readonly int y;
    private readonly int z;



    public int LengthSquared => x * x + y * y + z * z;
    public float Length => MathF.Sqrt(LengthSquared);
    public int LengthManhattan => System.Math.Abs(x) + System.Math.Abs(y) + System.Math.Abs(z);



    public static Vec3i Abs(Vec3i e) => new Vec3i(System.Math.Abs(e.x), System.Math.Abs(e.y), System.Math.Abs(e.z));

    public static Vec3i Min(Vec3i a, Vec3i b) => new Vec3i(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y), System.Math.Min(a.z, b.z));

    public static Vec3i Max(Vec3i a, Vec3i b) => new Vec3i(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y), System.Math.Max(a.z, b.z));

    public static Vec3i Clamp(Vec3i e, Vec3i min, Vec3i max) {
        return new Vec3i(
            e.x < min.x ? min.x : e.x > max.x ? max.x : e.x,
            e.y < min.y ? min.y : e.y > max.y ? max.y : e.y,
            e.z < min.z ? min.z : e.z > max.z ? max.z : e.z
        );
    }

    public static Vec3i Cross(Vec3i a, Vec3i b) => new Vec3i(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);

    public static int Dot(Vec3i a, Vec3i b) => a.x * b.x + a.y * b.y + a.z * b.z;

    public static int DistanceSquared(Vec3i a, Vec3i b) => (b - a).LengthSquared;

    public static int DistanceManhattan(Vec3i a, Vec3i b) => (b - a).LengthManhattan;



    public static Vec3i operator +(Vec3i a, Vec3i b) => new Vec3i(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Vec3i operator -(Vec3i a, Vec3i b) => new Vec3i(a.x - b.x, a.y - b.y, a.z - b.z);

    public static Vec3i operator -(Vec3i a) => new Vec3i(-a.x, -a.y, -a.z);

    public static Vec3i operator *(Vec3i a, Vec3i b) => new Vec3i(a.x * b.x, a.y * b.y, a.z * b.z);

    public static Vec3i operator *(Vec3i a, int b) => new Vec3i(a.x * b, a.y * b, a.z * b);

    public static Vec3i operator /(Vec3i a, Vec3i b) => new Vec3i(a.x / b.x, a.y / b.y, a.z / b.z);

    public static Vec3i operator /(Vec3i a, int b) => new Vec3i(a.x / b, a.y / b, a.z / b);
}