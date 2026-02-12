using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FrogLib;

public abstract class ResourceLibrary<T> : Module where T : class {

    private FastStack<ResourceInfo> resources = new();
    private Dictionary<int, int> indicesById = new();
    private Dictionary<string, int> idsByName = new();
    private int nextId;



    public T Get(string name) {
        if (!idsByName.TryGetValue(name, out int id)) NotFoundMessage(name);
        return resources[indicesById[id]].Resource;
    }

    public T Get(int id) {
        if (!indicesById.TryGetValue(id, out int index)) NotFoundMessage(id);
        return resources[index].Resource;
    }

    public int GetId(string name) {
        if (!idsByName.TryGetValue(name, out int id)) NotFoundMessage(name);
        return id;
    }



    public bool TryGet(string name, [NotNullWhen(true)] out T? resource) {
        if (!idsByName.TryGetValue(name, out int id)) {
            resource = null;
            return false;
        }

        resource = resources[indicesById[id]].Resource;
        return true;
    }

    public bool TryGet(int id, [NotNullWhen(true)] out T? resource) {
        if (!indicesById.TryGetValue(id, out int index)) {
            resource = null;
            return false;
        }

        resource = resources[index].Resource;
        return true;
    }

    public bool TryGetId(string name, out int id) => idsByName.TryGetValue(name, out id);



    protected int AddResource(string name, T resource) {
        if (idsByName.ContainsKey(name)) AlreadyPresentMessage(name);

        int id = nextId++;
        indicesById[id] = resources.Push(new ResourceInfo { Id = id, Resource = resource });
        idsByName[name] = id;
        return id;
    }



    protected T RemoveResource(string name) {
        if (!idsByName.Remove(name, out int id)) NotFoundMessage(name);
        indicesById.Remove(id, out int index);

        var resource = resources.Remove(index).Resource;

        for (int i = index; i < resources.Length; i++) {
            indicesById[resources[i].Id] = i;
        }

        return resource;
    }

    protected T RemoveResource(string name, Func<T, T, T> onAdjust) {
        if (!idsByName.Remove(name, out int id)) NotFoundMessage(name);
        indicesById.Remove(id, out int index);

        var resource = resources.Remove(index).Resource;

        for (int i = index; i < resources.Length; i++) {
            ref var other = ref resources[i];
            indicesById[other.Id] = i;
            other = new ResourceInfo { Id = other.Id, Resource = onAdjust(resource, other.Resource) };
        }

        return resource;
    }



    protected IEnumerable<T> GetResources() {
        for (int i = 0; i < resources.Length; i++) {
            yield return resources[i].Resource;
        }
    }



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual string AlreadyPresentMessage(string name) => $"Resource with the name '{name}' is already present in the library.";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual string NotFoundMessage(string name) => $"No resource with the name '{name}' found in the library.";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual string NotFoundMessage(int id) => $"No resource with the id '{id}' found in the library.";



    private struct ResourceInfo {
        public int Id;
        public T Resource;
    }
}