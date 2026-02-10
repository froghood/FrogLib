using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class Buffer : GLObject {

    public Buffer() : base(GL.CreateBuffer()) { }



    public void Use(BufferRangeTarget target, int index) {

        ThrowIfInvalid();

        GL.BindBufferBase(target, index, Handle);
    }
    public void Use(BufferTarget target) {

        ThrowIfInvalid();

        GL.BindBuffer(target, Handle);
    }



    public void BufferStorage(int size, BufferStorageFlags flags) => GL.NamedBufferStorage(Handle, size, 0, flags);

    public unsafe void BufferStorage<T>(Span<T> data, BufferStorageFlags flags) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferStorage(Handle, data.Length * sizeof(T), (nint)ptr, flags);
        }
    }

    public unsafe void BufferStorage<T>(Span<T> data, int size, BufferStorageFlags flags) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferStorage(Handle, size, (nint)ptr, flags);
        }
    }

    public unsafe void BufferStorage<T>(ReadOnlySpan<T> data, BufferStorageFlags flags) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferStorage(Handle, data.Length * sizeof(T), (nint)ptr, flags);
        }
    }

    public unsafe void BufferStorage<T>(ReadOnlySpan<T> data, int size, BufferStorageFlags flags) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferStorage(Handle, size, (nint)ptr, flags);
        }
    }



    public void BufferData(int size, BufferUsageHint hint = BufferUsageHint.StreamDraw) {

        ThrowIfInvalid();

        GL.NamedBufferData(Handle, size, 0, hint);
    }

    public unsafe void BufferData<T>(Span<T> data, BufferUsageHint hint = BufferUsageHint.StreamDraw) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferData(Handle, data.Length * sizeof(T), (nint)ptr, hint);
        }
    }

    public unsafe void BufferData<T>(Span<T> data, int size, BufferUsageHint hint = BufferUsageHint.StreamDraw) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferData(Handle, size, (nint)ptr, hint);
        }
    }

    public unsafe void BufferData<T>(ReadOnlySpan<T> data, BufferUsageHint hint = BufferUsageHint.StreamDraw) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferData(Handle, data.Length * sizeof(T), (nint)ptr, hint);
        }
    }

    public unsafe void BufferData<T>(ReadOnlySpan<T> data, int size, BufferUsageHint hint = BufferUsageHint.StreamDraw) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferData(Handle, size, (nint)ptr, hint);
        }
    }



    public unsafe void BufferSubData<T>(Span<T> data) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferSubData(Handle, 0, data.Length * sizeof(T), (nint)ptr);
        }
    }

    public unsafe void BufferSubData<T>(Span<T> data, int offset) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferSubData(Handle, offset, data.Length * sizeof(T), (nint)ptr);
        }
    }

    public unsafe void BufferSubData<T>(Span<T> data, int offset, int size) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferSubData(Handle, offset, size, (nint)ptr);
        }
    }

    public unsafe void BufferSubData<T>(ReadOnlySpan<T> data) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferSubData(Handle, 0, data.Length * sizeof(T), (nint)ptr);
        }
    }

    public unsafe void BufferSubData<T>(ReadOnlySpan<T> data, int offset) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferSubData(Handle, offset, data.Length * sizeof(T), (nint)ptr);
        }
    }

    public unsafe void BufferSubData<T>(ReadOnlySpan<T> data, int offset, int size) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.NamedBufferSubData(Handle, offset, size, (nint)ptr);
        }
    }



    public unsafe void GetData<T>(Span<T> data, int offset = 0) where T : unmanaged {

        ThrowIfInvalid();

        fixed (T* ptr = &data[0]) {
            GL.GetNamedBufferSubData(Handle, offset, data.Length * sizeof(T), (nint)ptr);
        }
    }

    public unsafe T GetData<T>(int offset = 0) where T : unmanaged {

        ThrowIfInvalid();

        T data = default;
        GL.GetNamedBufferSubData<T>(Handle, offset, sizeof(T), ref data);
        return data;
    }



    public void CopyData(Buffer other, int size, int sourceOffset = 0, int destOffset = 0) {

        ThrowIfInvalid();

        GL.CopyNamedBufferSubData(Handle, other.Handle, sourceOffset, destOffset, size);
    }

    protected override void Delete() {
        GL.DeleteBuffer(Handle);
    }
}