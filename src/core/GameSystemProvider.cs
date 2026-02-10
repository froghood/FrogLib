namespace FrogLib;

internal class ModuleProvider {



    private FastStack<Module> modules = new();
    private Dictionary<Type, Module> moduleDict = new();

    internal T Get<T>() where T : Module {
        var type = typeof(T);

        if (moduleDict.TryGetValue(type, out var module)) {
            return (T)module;
        }

        throw new InvalidOperationException($"Can't get module \"{type.Name}\"; it is not registered");
    }

    internal T Register<T>(T module) where T : Module {

        var type = typeof(T);

        if (moduleDict.ContainsKey(type)) {
            throw new InvalidOperationException($"Module \"{type.Name}\" already registered");
        }

        modules.Push(module);
        moduleDict[type] = module;

        return module;
    }

    internal void Startup() {
        for (int i = 0; i < modules.Length; i++) {
            modules[i].Startup();
        }
    }


    internal void Update() {
        for (int i = 0; i < modules.Length; i++) {
            modules[i].PreUpdate();
        }

        for (int i = 0; i < modules.Length; i++) {
            modules[i].Update();
        }

        for (int i = 0; i < modules.Length; i++) {
            modules[i].PostUpdate();
        }
    }

    internal void Render(float alpha) {
        for (int i = 0; i < modules.Length; i++) {
            modules[i].PreRender(alpha);
        }

        for (int i = 0; i < modules.Length; i++) {
            modules[i].Render(alpha);
        }

        for (int i = 0; i < modules.Length; i++) {
            modules[i].PostRender(alpha);
        }
    }

    internal void Shutdown() {
        for (int i = 0; i < modules.Length; i++) {
            modules[i].Shutdown();
        }
    }
}