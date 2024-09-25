
namespace FrogLib;

internal class GameSystemProvider {



    private List<GameSystem> systems;
    private Dictionary<Type, GameSystem> registeredSystems;

    public GameSystemProvider() {

        systems = new List<GameSystem>();
        registeredSystems = new Dictionary<Type, GameSystem>();
    }



    internal T Get<T>() where T : GameSystem {
        var type = typeof(T);

        if (!registeredSystems.ContainsKey(type)) {
            throw new Exception($"Can't get system \"{type.Name}\"; it is not registered");
        }

        return (T)registeredSystems[type];
    }

    internal T Register<T>() where T : GameSystem {

        var type = typeof(T);

        if (registeredSystems.ContainsKey(type)) {
            throw new Exception($"System \"{type.Name}\" already registered");
        }

        var system = (T)Activator.CreateInstance(type)!;

        system.Startup();

        systems.Add(system);
        registeredSystems[type] = system;

        return system;
    }



    protected internal void Poll() {
        for (int i = 0; i < systems.Count; i++) {
            systems[i].Poll();
        }
    }

    protected internal void Update() {
        for (int i = 0; i < systems.Count; i++) {
            systems[i].Update();
        }
    }

    protected internal void Render(float alpha) {
        for (int i = 0; i < systems.Count; i++) {
            systems[i].Render(alpha);
        }
    }

    protected internal void Shutdown() {
        for (int i = 0; i < systems.Count; i++) {
            systems[i].Shutdown();
        }
    }
}