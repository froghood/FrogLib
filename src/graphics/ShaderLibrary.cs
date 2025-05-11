using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class ShaderLibrary : GameSystem {

    private Dictionary<string, Shader> shaders = new();



    public void Add(string name, Shader shader) {
        ThrowIfPresent(name);
        shaders.Add(name, shader);
    }

    public Shader Get(string name) => GetOrThrowIfNotPresent(name);

    public void Use(string name) {
        var shader = GetOrThrowIfNotPresent(name);
        shader.Use();
    }



    // vec1
    public void Uniform(string shaderName, string uniformName, float value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, int value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, double value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // vec2
    public void Uniform(string shaderName, string uniformName, Vector2 value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Vector2i value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // vec3
    public void Uniform(string shaderName, string uniformName, Vector3 value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Vector3i value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

    // vec4
    public void Uniform(string shaderName, string uniformName, Vector4 value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);
    public void Uniform(string shaderName, string uniformName, Vector4i value) => GetOrThrowIfNotPresent(shaderName).Uniform(uniformName, value);

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
        if (shaders.ContainsKey(name)) throw new Exception($"Shader with the name {name} already present in the library.");
    }

    private Shader GetOrThrowIfNotPresent(string name) {
        if (shaders.TryGetValue(name, out var shader)) return shader;
        throw new Exception($"No shader with the name {name} found in the library. If you are using default renderables, be sure to load default shaders.");
    }

}