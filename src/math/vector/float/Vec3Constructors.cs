
namespace FrogLib.Mathematics;

public readonly partial struct Vec3 : IEquatable<Vec3> {

    public Vec3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3(float n) : this(n, n, n) { }

    public Vec3(Vec2i v, float z) : this(v.X, v.Y, z) { }
    public Vec3(float x, Vec2i v) : this(x, v.X, v.Y) { }



    public static Vec3 Zero => new Vec3(0f, 0f, 0f);
    public static Vec3 One => new Vec3(1f, 1f, 1f);
    public static Vec3 UnitX => new Vec3(1f, 0f, 0f);
    public static Vec3 UnitY => new Vec3(0f, 1f, 0f);
    public static Vec3 UnitZ => new Vec3(0f, 0f, 1f);



    public static implicit operator Vec3(Vec3i v) => new Vec3(v.X, v.Y, v.Z);

    public static implicit operator Vec3(OpenTK.Mathematics.Vector3 v) => new Vec3(v.X, v.Y, v.Z);
    public static implicit operator OpenTK.Mathematics.Vector3(Vec3 v) => new OpenTK.Mathematics.Vector3(v.x, v.y, v.z);

    public static implicit operator Vec3(OpenTK.Mathematics.Vector3i v) => new Vec3(v.X, v.Y, v.Z);
}