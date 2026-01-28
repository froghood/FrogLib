using System.Runtime.CompilerServices;

namespace FrogLib;

public abstract class GLObject : IDisposable {


    public int Id => id;
    public bool IsDeleted => id == 0;


    private int id;

    ~GLObject() => Dispose();

    public GLObject(int id) {
        this.id = id;
    }

    public void Dispose() {
        if (id == 0) return;

        GC.SuppressFinalize(this);
        Delete();

        id = 0;
    }

    protected abstract void Delete();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfInvalid() {
        if (id == 0) throw new OpenGLObjectInvalidException($"{GetType().Name} is deleted.");
    }
}