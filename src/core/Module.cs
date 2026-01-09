
namespace FrogLib;

public abstract class Module {

    protected internal virtual void Startup() { }

    protected internal virtual void PreUpdate() { }
    protected internal virtual void Update() { }
    protected internal virtual void PostUpdate() { }

    protected internal virtual void PreRender(float alpha) { }
    protected internal virtual void Render(float alpha) { }
    protected internal virtual void PostRender(float alpha) { }

    protected internal virtual void Shutdown() { }

}