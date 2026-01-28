using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;



public abstract class Texture : GLObject {

    public TextureTarget Target => target;
    public int Levels => levels;
    public SizedInternalFormat Format => format;


    private TextureTarget target;
    private int levels;
    private SizedInternalFormat format;


    private bool isHandleLocked = false;



    public Texture(TextureTarget target, int levels, Vec3i size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(target)) {

        levels = int.Clamp(levels, 1, int.Log2(size.MaxComponent) + 1);

        this.target = target;
        this.levels = levels;
        this.format = format;

        GL.TextureStorage3D(Id, levels, format, size.X, size.Y, size.Z);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Id);
        }
    }

    public Texture(TextureTarget target, int levels, Vec2i size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(target)) {
        levels = int.Clamp(levels, 1, int.Log2(size.MaxComponent) + 1);

        this.target = target;
        this.levels = levels;
        this.format = format;

        GL.TextureStorage2D(Id, levels, format, size.X, size.Y);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Id);
        }
    }

    public Texture(TextureTarget target, int levels, int size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(target)) {

        levels = int.Clamp(levels, 1, int.Log2(size) + 1);

        this.target = target;
        this.levels = levels;
        this.format = format;

        GL.TextureStorage1D(Id, levels, format, size);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Id);
        }
    }



    public void SetParam(in TextureParameter parameter) {

        ThrowIfInvalid();
        if (isHandleLocked) throw new InvalidOperationException("Texture is immutable because a texture/image/sampler handle was previously created.");

        parameter.Apply(Id);
    }



    public void Use(int unit) {
        ThrowIfInvalid();
        GL.BindTextureUnit(unit, Id);
    }

    public void UseImage(int unit, TextureAccess access) {
        ThrowIfInvalid();
        GL.BindImageTexture(unit, Id, 0, false, 0, access, format);
    }



    public TextureHandle CreateTextureHandle() {

        ThrowIfInvalid();

        isHandleLocked = true;
        return new TextureHandle(GL.Arb.GetTextureHandle(Id));
    }

    public ImageHandle CreateImageHandle(int level, bool layered, int layer, PixelFormat format) {

        ThrowIfInvalid();

        isHandleLocked = true;
        return new ImageHandle(GL.Arb.GetImageHandle(Id, level, layered, layer, format));
    }

    protected override void Delete() {
        GL.DeleteTexture(Id);
    }
}