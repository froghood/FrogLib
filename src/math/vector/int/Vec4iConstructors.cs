
namespace FrogLib.Mathematics;

public readonly partial record struct Vec4i {

    public Vec4i(int x, int y, int z, int w) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vec4i(int n) : this(n, n, n, n) { }

    public Vec4i(Vec3i v, int w) : this(v.X, v.Y, v.Z, w) { }
    public Vec4i(int x, Vec3i v) : this(x, v.X, v.Y, v.Z) { }
    public Vec4i(Vec2i v, int z, int w) : this(v.X, v.Y, z, w) { }
    public Vec4i(int x, Vec2i v, int w) : this(x, v.X, v.Y, w) { }
    public Vec4i(int x, int y, Vec2i v) : this(x, y, v.X, v.Y) { }
    public Vec4i(Vec2i v, Vec2i w) : this(v.X, v.Y, w.X, w.Y) { }



    public static Vec4i Zero => new Vec4i(0, 0, 0, 0);
    public static Vec4i One => new Vec4i(1, 1, 1, 1);
    public static Vec4i UnitX => new Vec4i(1, 0, 0, 0);
    public static Vec4i UnitY => new Vec4i(0, 1, 0, 0);
    public static Vec4i UnitZ => new Vec4i(0, 0, 1, 0);
    public static Vec4i UnitW => new Vec4i(0, 0, 0, 1);



    public static implicit operator Vec4i(OpenTK.Mathematics.Vector4i v) => new Vec4i(v.X, v.Y, v.Z, v.W);
    public static implicit operator OpenTK.Mathematics.Vector4i(Vec4i v) => new OpenTK.Mathematics.Vector4i(v.X, v.Y, v.Z, v.W);

    public static unsafe explicit operator int*(Vec4i v) => &v.x;
}