
namespace FrogLib.Mathematics;

public readonly partial record struct Vec3i {

    public Vec3i(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3i(int n) : this(n, n, n) { }


    public Vec3i(Vec2i v, int z) : this(v.X, v.Y, z) { }
    public Vec3i(int x, Vec2i v) : this(x, v.X, v.Y) { }



    public static Vec3i Zero => new Vec3i(0, 0, 0);
    public static Vec3i One => new Vec3i(1, 1, 1);
    public static Vec3i UnitX => new Vec3i(1, 0, 0);
    public static Vec3i UnitY => new Vec3i(0, 1, 0);
    public static Vec3i UnitZ => new Vec3i(0, 0, 1);



    public static implicit operator Vec3i(OpenTK.Mathematics.Vector3i v) => new Vec3i(v.X, v.Y, v.Z);
    public static implicit operator OpenTK.Mathematics.Vector3(Vec3i v) => new OpenTK.Mathematics.Vector3i(v.x, v.y, v.z);

    public static unsafe explicit operator int*(Vec3i v) => &v.x;
}