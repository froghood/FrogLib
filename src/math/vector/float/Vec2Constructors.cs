
namespace FrogLib.Mathematics;

public readonly partial struct Vec2 {

    public Vec2(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public Vec2(float n) : this(n, n) { }



    public static Vec2 Zero => new Vec2(0, 0);
    public static Vec2 One => new Vec2(1, 1);
    public static Vec2 UnitX => new Vec2(1, 0);
    public static Vec2 UnitY => new Vec2(0, 1);



    public static implicit operator Vec2(Vec2i v) => new Vec2(v.X, v.Y);

    public static implicit operator Vec2(OpenTK.Mathematics.Vector2 v) => new Vec2(v.X, v.Y);
    public static implicit operator OpenTK.Mathematics.Vector2(Vec2 v) => new OpenTK.Mathematics.Vector2(v.X, v.Y);

    public static implicit operator Vec2(OpenTK.Mathematics.Vector2i v) => new Vec2(v.X, v.Y);
}