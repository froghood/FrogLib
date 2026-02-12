using System.Diagnostics.CodeAnalysis;
using FrogLib.Mathematics;
using OpenTK.Mathematics;

namespace FrogLib;

public class ShaderLibrary : ResourceLibrary<Shader> {

    public int Add(string name, Shader shader) => AddResource(name, shader);

    public void Remove(string name, bool dispose = true) {
        var shader = RemoveResource(name);

        if (dispose) shader.Dispose();
    }



    public int LoadFile(string path, string name) {
        var shader = Shader.FromFile(path, name);
        return AddResource(name, shader);
    }

    public int LoadFile(string path, string name, string outputDir) {
        var shader = Shader.FromFile(path, name, outputDir);
        return AddResource(name, shader);
    }

    public void LoadFiles(string dir, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            LoadFile(path, name);
        }
    }

    public void LoadFiles(string dir, string outputDir, bool recursive = false) {
        foreach (var path in Directory.EnumerateFiles(dir, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)) {

            string name = Path.ChangeExtension(PathExt.ToUnixPath(Path.GetRelativePath(dir, path)), null);

            LoadFile(path, name, outputDir);
        }
    }



    protected internal override void Shutdown() {
        foreach (var resource in GetResources()) {
            resource.Dispose();
        }
    }



    protected override string AlreadyPresentMessage(string name) => $"Shader with the name '{name}' is already present in the library.";
    protected override string NotFoundMessage(string name) => $"No shader with the name '{name}' found in the library.";
    protected override string NotFoundMessage(int id) => $"No shader with the id '{id}' found in the library.";
}