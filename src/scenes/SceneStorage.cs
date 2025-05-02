namespace FrogLib;

public class SceneStorage : GameSystem {

    internal Scene Current { get => current; }


    private Scene current;
    private Dictionary<Type, Scene> scenes;

    internal SceneStorage() {
        scenes = new Dictionary<Type, Scene>();
        current = new EmptyScene();
    }

    public void Change<T>(bool saveCurrent = false, params object[] args) where T : Scene {
        var type = typeof(T);

        if (scenes.TryGetValue(type, out var savedScene)) {
            scenes.Remove(type);

            savedScene.Activate();

            if (saveCurrent) {
                current.Deactivate();
                scenes.Add(current.GetType(), current);
            } else {
                current.Shutdown();
            }

            current = savedScene;

        } else {

            var scene = (T)Activator.CreateInstance(type, args)!;
            scene.Startup();

            if (saveCurrent) {
                current.Deactivate();
                scenes.Add(current.GetType(), current);
            } else {
                current.Shutdown();
            }

            current = scene;
        }
    }

    protected internal override void PreUpdate() => Current.PreUpdate();
    protected internal override void Update() => Current.Update();
    protected internal override void PostUpdate() => Current.PostUpdate();
    protected internal override void PreRender(float alpha) => Current.PreRender(alpha);
    protected internal override void Render(float alpha) => Current.Render(alpha);
    protected internal override void PostRender(float alpha) => Current.PostRender(alpha);
}