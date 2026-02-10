using System.Diagnostics.CodeAnalysis;
using FrogLib.Mathematics;
using OpenTK.Mathematics;

namespace FrogLib;

public class ShaderLibrary : Module {

    private Dictionary<string, Shader> shaders = new();



    public void Add(string name, Shader shader) {

        if (shaders.ContainsKey(name)) {
            throw new ArgumentException($"Cannot add shader. Shader with the name \"{name}\" already present in the library.");
        }

        shaders.Add(name, shader);
    }



    public void LoadFile(string path, string name) {

        if (shaders.ContainsKey(name)) {
            throw new ArgumentException($"Cannot load shader. Shader with the name \"{name}\" already present in the library.");
        }

        var shader = Shader.FromFile(path, name);

        shaders.Add(name, shader);
    }

    public void LoadFile(string path, string name, string outputDir) {
        var shader = Shader.FromFile(path, name, outputDir);
        Add(name, shader);
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



    public Shader GetShader(string name) => GetOrThrowIfNotPresent(name);
    public bool TryGetShader(string name, [NotNullWhen(true)] out Shader? shader) {
        return shaders.TryGetValue(name, out shader);
    }

    public void Use(string name) {
        var shader = GetOrThrowIfNotPresent(name);
        shader.Use();
    }



    // vec1
    public void Uniform(string shaderName, string uniformName, float value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, int value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, double value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // vec2
    public void Uniform(string shaderName, string uniformName, Vec2 value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Vec2i value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // vec3
    public void Uniform(string shaderName, string uniformName, Vec3 value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Vec3i value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // vec4
    public void Uniform(string shaderName, string uniformName, Vec4 value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Vec4i value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // mat2
    public void Uniform(string shaderName, string uniformName, Matrix2 value, bool transpose = false) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Matrix2d value, bool transpose = false) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // mat3
    public void Uniform(string shaderName, string uniformName, Matrix3 value, bool transpose = false) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Matrix3d value, bool transpose = false) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // mat4
    public void Uniform(string shaderName, string uniformName, Matrix4 value, bool transpose = false) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Matrix4d value, bool transpose = false) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);



    private void ThrowIfPresent(string name) {
        if (shaders.ContainsKey(name)) throw new ArgumentException($"Shader with the name {name} already present in the library.");
    }

    private Shader GetOrThrowIfNotPresent(string name) {
        if (shaders.TryGetValue(name, out var shader)) return shader;
        throw new ArgumentException($"No shader with the name {name} found in the library.");
    }

    protected internal override void Shutdown() {
        foreach (var shader in shaders.Values) shader.Dispose();
    }
}