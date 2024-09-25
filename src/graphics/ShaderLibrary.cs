using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class ShaderLibrary : GameSystem {


    private Dictionary<string, int> programs = new();

    private Dictionary<int, Dictionary<string, int>> shaderLocations = new();

    private int currentlyBoundProgram = 0;



    internal ShaderLibrary() { }



    public void LoadShaderFromPath(string shaderPath, ShaderType shaderType) {

        var name = Path.GetFileNameWithoutExtension(shaderPath);
        var source = File.ReadAllText(shaderPath);

        LoadShader(name, shaderType, source);
    }



    public void LoadShader(string name, ShaderType shaderType, string source) {
        int shader = GL.CreateShader(shaderType);

        GL.ShaderSource(shader, source);
        _ = CompileShader(shader);


        programs.TryAdd(name, GL.CreateProgram());

        programs.TryGetValue(name, out int program);

        GL.AttachShader(program, shader);

        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0) {
            string infoLog = GL.GetProgramInfoLog(program);
            Log.Info(infoLog);
        }
    }



    public void LoadDefaultShaders() {
        LoadShader("rectangle", ShaderType.VertexShader, DefaultShaders.RectangleVertexShader);
        LoadShader("rectangle", ShaderType.FragmentShader, DefaultShaders.RectangleFragmentShader);
        LoadShader("circle", ShaderType.VertexShader, DefaultShaders.CircleVertexShader);
        LoadShader("circle", ShaderType.FragmentShader, DefaultShaders.CircleFragmentShader);
        LoadShader("line", ShaderType.VertexShader, DefaultShaders.LineVertexShader);
        LoadShader("line", ShaderType.FragmentShader, DefaultShaders.LineFragmentShader);
        LoadShader("pixelsprite", ShaderType.VertexShader, DefaultShaders.PixelSpriteVertexShader);
        LoadShader("pixelsprite", ShaderType.FragmentShader, DefaultShaders.PixelSpriteFragmentShader);
    }



    public void UseShader(string name) {
        if (programs.TryGetValue(name, out int program)) {
            if (program == currentlyBoundProgram) return;

            GL.UseProgram(program);
            currentlyBoundProgram = program;
        } else {
            Log.Warn($"shader: \"{name}\" not loaded. if you are using default renderables be sure to load default shaders");
        }
    }



    public void Uniform(string name, float value) { if (GetUniformLocation(name, out int location)) GL.Uniform1(location, value); }
    public void Uniform(string name, Vector2 value) { if (GetUniformLocation(name, out int location)) GL.Uniform2(location, value); }
    public void Uniform(string name, Vector3 value) { if (GetUniformLocation(name, out int location)) GL.Uniform3(location, value); }
    public void Uniform(string name, Vector4 value) { if (GetUniformLocation(name, out int location)) GL.Uniform4(location, value); }
    public void Uniform(string name, int value) { if (GetUniformLocation(name, out int location)) GL.Uniform1(location, value); }
    public void Uniform(string name, Vector2i value) { if (GetUniformLocation(name, out int location)) GL.Uniform2(location, value); }
    public void Uniform(string name, Vector3i value) { if (GetUniformLocation(name, out int location)) GL.Uniform3(location, value); }
    public void Uniform(string name, Vector4i value) { if (GetUniformLocation(name, out int location)) GL.Uniform4(location, value); }
    public void Uniform(string name, Color4 value) { if (GetUniformLocation(name, out int location)) GL.Uniform4(location, value); }
    public void Uniform(string name, bool value) { if (GetUniformLocation(name, out int location)) GL.Uniform1(location, Convert.ToInt32(value)); }
    public void Uniform(string name, Matrix2 value) { if (GetUniformLocation(name, out int location)) GL.UniformMatrix2(location, true, ref value); }
    public void Uniform(string name, Matrix4 value) { if (GetUniformLocation(name, out int location)) GL.UniformMatrix4(location, false, ref value); }

    private bool GetUniformLocation(string name, out int location) {

        if (shaderLocations.ContainsKey(currentlyBoundProgram)) {
            if (shaderLocations[currentlyBoundProgram].TryGetValue(name, out var _location)) {
                location = _location;
                return true;
            } else {
                location = GL.GetUniformLocation(currentlyBoundProgram, name);
                if (location >= 0) {
                    shaderLocations[currentlyBoundProgram][name] = location;
                    return true;
                } else {
                    return false;
                }
            }
        } else {
            location = GL.GetUniformLocation(currentlyBoundProgram, name);
            if (location >= 0) {
                shaderLocations[currentlyBoundProgram] = new Dictionary<string, int> {
                    {name, location},
                };
                return true;
            } else {
                return false;
            }
        }
    }

    private int CompileShader(int handle) {

        GL.CompileShader(handle);
        GL.GetShader(handle, ShaderParameter.CompileStatus, out int status);

        if (status == 0) {
            string infoLog = GL.GetShaderInfoLog(handle);
        }

        return status;
    }
}