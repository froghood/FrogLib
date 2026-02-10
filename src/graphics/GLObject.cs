using System.Runtime.CompilerServices;

namespace FrogLib;

public abstract class GLObject : IDisposable {

    /// <summary>
    /// The OpenGL object handle
    /// </summary>
    public int Handle => handle;
    public bool IsDeleted => handle == 0;


    private int handle;

    ~GLObject() => Dispose();

    public GLObject(int id) {
        handle = id;
    }

    public void Dispose() {
        if (handle == 0) return;

        GC.SuppressFinalize(this);
        Delete();

        handle = 0;
    }

    protected abstract void Delete();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ThrowIfInvalid() {
        if (handle == 0) throw new OpenGLObjectInvalidException($"{GetType().Name} is deleted.");
    }
}