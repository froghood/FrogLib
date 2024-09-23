public abstract class Scene {

    /// <summary>
    /// Called once when loading the scene for the first time
    /// </summary>
    public virtual void Startup() { }

    /// <summary>
    /// Called when the scene is being saved before changing to a new scene
    /// </summary>
    public virtual void Deactivate() { }

    /// <summary>
    /// Called when the scene is being loaded from a saved state
    /// </summary>
    public virtual void Activate() { }

    /// <summary>
    /// Called when the scene is being changed without saving
    /// </summary>
    public virtual void Shutdown() { }

    /// <summary>
    /// Called when inputs are polled
    /// </summary>
    public virtual void Poll() { }

    /// <summary>
    /// Called every frame when the game updates
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Called every frame when the game renderer
    /// </summary>
    /// <param name="alpha"> 
    /// a value between 0 and 1 measuring the time between the previous and next fixed updates
    /// </param>
    public virtual void Render(float alpha) { }


}

internal class EmptyScene : Scene { }