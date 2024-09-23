
using OpenTK.Windowing.Desktop;

namespace FrogLib;

public abstract class Renderer : IDisposable {

    protected IGLFWGraphicsContext Context { get; }

    public Renderer(IGLFWGraphicsContext context) {
        Context = context;
    }

    public virtual void Init() { }
    public abstract void Render(IRenderable[] renderables);

    public virtual void Dispose() { }
}