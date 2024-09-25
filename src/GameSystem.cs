using FrogLib;

public abstract class GameSystem {

    protected internal virtual void Startup() { }
    protected internal virtual void Poll() { }
    protected internal virtual void Update() { }
    protected internal virtual void Render(float alpha) { }
    protected internal virtual void Shutdown() { }


}