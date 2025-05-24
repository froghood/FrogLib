
namespace FrogLib.Mathematics;

public readonly partial record struct Vec4 {

    public Vec4(float x, float y, float z, float w) {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vec4(float n) : this(n, n, n, n) { }

    public Vec4(Vec3 v, float w) : this(v.X, v.Y, v.Z, w) { }
    public Vec4(float x, Vec3 v) : this(x, v.X, v.Y, v.Z) { }
    public Vec4(Vec2 v, float z, float w) : this(v.X, v.Y, z, w) { }
    public Vec4(float x, Vec2 v, float w) : this(x, v.X, v.Y, w) { }
    public Vec4(float x, float y, Vec2 v) : this(x, y, v.X, v.Y) { }
    public Vec4(Vec2 v, Vec2 w) : this(v.X, v.Y, w.X, w.Y) { }



    public static Vec4 Zero => new Vec4(0f, 0f, 0f, 0f);
    public static Vec4 One => new Vec4(1f, 1f, 1f, 1f);
    public static Vec4 UnitX => new Vec4(1f, 0f, 0f, 0f);
    public static Vec4 UnitY => new Vec4(0f, 1f, 0f, 0f);
    public static Vec4 UnitZ => new Vec4(0f, 0f, 1f, 0f);
    public static Vec4 UnitW => new Vec4(0f, 0f, 0f, 1f);



    public static implicit operator Vec4(Vec4i v) => new Vec4(v.X, v.Y, v.Z, v.W);

    public static implicit operator Vec4(OpenTK.Mathematics.Vector4 v) => new Vec4(v.X, v.Y, v.Z, v.W);
    public static implicit operator OpenTK.Mathematics.Vector4(Vec4 v) => new OpenTK.Mathematics.Vector4(v.x, v.y, v.z, v.w);

    public static explicit operator Vec4(OpenTK.Mathematics.Color4 color) => new Vec4(color.R, color.G, color.B, color.A);
    public static explicit operator OpenTK.Mathematics.Color4(Vec4 v) => new OpenTK.Mathematics.Color4(v.x, v.y, v.z, v.w);

    public static unsafe explicit operator float*(Vec4 v) => &v.x;
}