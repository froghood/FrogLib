using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;



public abstract class Texture : GLObject {

    public TextureTarget Target => target;
    public int Levels => levels;
    public SizedInternalFormat Format => format;


    private TextureTarget target;
    private int levels;
    private SizedInternalFormat format;



    private FastStack<BindlessHandle> handles = new();
    private int? handleIndex;
    private Dictionary<Sampler, int> samplerHandleIndices = new();
    private Dictionary<int, int> imageHandleIndices = new();
    private bool isHandleLocked = false;



    public Texture(TextureTarget target, int levels, Vec3i size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(target)) {

        levels = int.Clamp(levels, 1, int.Log2(size.MaxComponent) + 1);

        this.target = target;
        this.levels = levels;
        this.format = format;

        GL.TextureStorage3D(Handle, levels, format, size.X, size.Y, size.Z);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Handle);
        }
    }

    public Texture(TextureTarget target, int levels, Vec2i size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(target)) {
        levels = int.Clamp(levels, 1, int.Log2(size.MaxComponent) + 1);

        this.target = target;
        this.levels = levels;
        this.format = format;

        GL.TextureStorage2D(Handle, levels, format, size.X, size.Y);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Handle);
        }
    }

    public Texture(TextureTarget target, int levels, int size, SizedInternalFormat format, params TextureParameter[] parameters)
    : base(GL.CreateTexture(target)) {

        levels = int.Clamp(levels, 1, int.Log2(size) + 1);

        this.target = target;
        this.levels = levels;
        this.format = format;

        GL.TextureStorage1D(Handle, levels, format, size);

        for (int i = 0; i < parameters.Length; i++) {
            parameters[i].Apply(Handle);
        }
    }



    public void SetParam(in TextureParameter parameter) {

        ThrowIfInvalid();
        if (isHandleLocked) throw new InvalidOperationException("Texture is immutable because a texture/image/sampler handle was previously created.");

        parameter.Apply(Handle);
    }



    public void Use(int unit) {
        ThrowIfInvalid();
        GL.BindTextureUnit(unit, Handle);
    }

    public void Use(int unit, Sampler sampler) {
        ThrowIfInvalid();
        sampler.Use(unit);
        GL.BindTextureUnit(unit, Handle);
    }

    public void UseImage(int unit, TextureAccess access) {
        ThrowIfInvalid();
        GL.BindImageTexture(unit, Handle, 0, false, 0, access, format);
    }



    public long GetBindlessHandle() {
        ThrowIfInvalid();

        isHandleLocked = true;

        if (handleIndex.HasValue) {

            ref var handle = ref handles[handleIndex.Value];
            handle.MakeResident();
            return handle.Value;

        } else {

            var handle = new BindlessHandle(GL.Arb.GetTextureHandle(Handle));
            handle.MakeResident();
            handleIndex = handles.Push(handle);

            Log.Info($"Creating bindless texture handle. ({handle.Value})");
            return handle.Value;
        }
    }

    public long GetBindlessSamplerHandle(Sampler sampler) {
        ThrowIfInvalid();
        sampler.ThrowIfInvalid();

        isHandleLocked = true;

        if (samplerHandleIndices.TryGetValue(sampler, out var index)) {

            ref var handle = ref handles[index];
            handle.MakeResident();
            return handle.Value;

        } else {


            var handle = new BindlessHandle(GL.Arb.GetTextureSamplerHandle(Handle, sampler.Handle));
            handle.MakeResident();
            samplerHandleIndices[sampler] = handles.Push(handle);

            Log.Info($"Creating bindless sampler handle. ({handle.Value})");
            return handle.Value;

        }
    }

    public long GetBindlessImageHandle(int level, bool layered, int layer, PixelFormat format, TextureAccess access) {
        ThrowIfInvalid();

        isHandleLocked = true;

        int hash = HashCode.Combine(level, layered, layer, format);

        if (imageHandleIndices.TryGetValue(hash, out var index)) {

            ref var handle = ref handles[index];
            handle.MakeImageResident(access);
            return handle.Value;

        } else {

            var handle = new BindlessHandle(GL.Arb.GetImageHandle(Handle, level, layered, layer, format), true);
            handle.MakeImageResident(access);

            imageHandleIndices[hash] = handles.Push(handle);
            return handle.Value;
        }
    }



    public void DisableHandle() {
        ThrowIfInvalid();

        if (!handleIndex.HasValue) return;
        handles[handleIndex.Value].MakeNonResident();
    }

    public void DisableSamplerHandle(Sampler sampler) {
        ThrowIfInvalid();

        if (!samplerHandleIndices.TryGetValue(sampler, out var index)) return;
        handles[index].MakeNonResident();
    }

    public void DisableImageHandle(int level, bool layered, int layer, PixelFormat format) {
        ThrowIfInvalid();

        int hash = HashCode.Combine(level, layered, layer, format);

        if (!imageHandleIndices.TryGetValue(hash, out var index)) return;
        handles[index].MakeNonResident();
    }



    protected override void Delete() {

        for (int i = 0; i < handles.Length; i++) {
            ref var handle = ref handles[i];
            if (handle.IsImage) handle.MakeImageNonResident();
            else handle.MakeNonResident();
        }

        GL.DeleteTexture(Handle);
    }


    private struct BindlessHandle {

        public long Value => value;
        public bool IsImage => isImage;

        public BindlessHandle(long value, bool isImage = false) {
            this.value = value;
            this.isImage = isImage;
        }


        private long value;
        private bool isResident;
        private bool isImage;

        public void MakeResident() {
            if (isResident) return;
            GL.Arb.MakeTextureHandleResident(value);
            isResident = true;
        }

        public void MakeImageResident(TextureAccess access) {
            if (isResident) return;
            GL.Arb.MakeImageHandleResident(value, (All)access);
            isResident = true;
        }

        public void MakeNonResident() {
            if (!isResident) return;
            GL.Arb.MakeTextureHandleNonResident(value);
            isResident = false;
        }

        public void MakeImageNonResident() {
            if (!isResident) return;
            GL.Arb.MakeImageHandleNonResident(value);
            isResident = false;
        }
    }
}