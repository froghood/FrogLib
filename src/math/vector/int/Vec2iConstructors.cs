
namespace FrogLib.Mathematics;

public readonly partial struct Vec2i : IEquatable<Vec2i> {

    public Vec2i(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vec2i(int n) : this(n, n) { }



    public static Vec2i Zero => new Vec2i(0, 0);
    public static Vec2i One => new Vec2i(1, 1);
    public static Vec2i UnitX => new Vec2i(1, 0);
    public static Vec2i UnitY => new Vec2i(0, 1);



    public static implicit operator Vec2i(OpenTK.Mathematics.Vector2i v) => new Vec2i(v.X, v.Y);
    public static implicit operator OpenTK.Mathematics.Vector2i(Vec2i v) => new OpenTK.Mathematics.Vector2i(v.X, v.Y);

    public static unsafe explicit operator int*(Vec2i v) => &v.x;
}