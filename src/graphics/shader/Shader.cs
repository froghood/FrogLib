using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

public class Shader : GLObject {

    public string Name => name;

    private string name;
    private Dictionary<string, int> uniformLocations = new();



    internal Shader(string name, ReadOnlySpan<ShaderSource> sources)
    : base(GL.CreateProgram()) {
        this.name = name;
        Link(sources);
    }



    public static Shader FromSource(string source, string name) {
        using TextReader text = new StringReader(source);
        return ShaderReader.Read(text, string.Empty, name, string.Empty);
    }

    public static Shader FromSource(string source, string name, string outputDir) {
        using TextReader text = new StringReader(source);
        return ShaderReader.Read(text, string.Empty, name, outputDir);
    }

    public static Shader FromFile(string path, string name) {
        using TextReader text = File.OpenText(path);
        return ShaderReader.Read(text, path, name, string.Empty);
    }

    public static Shader FromFile(string path, string name, string outputDir) {
        using TextReader text = File.OpenText(path);
        return ShaderReader.Read(text, path, name, outputDir);
    }



    private void Link(ReadOnlySpan<ShaderSource> sources) {

        Span<int> shaders = stackalloc int[sources.Length];

        for (int i = 0; i < sources.Length; i++) {
            var source = sources[i];

            int shader = GL.CreateShader(source.Type);
            shaders[i] = shader;

            GL.ShaderSource(shader, source.Source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int compileStatus);
            if (compileStatus == 0) throw new ShaderCompilationException($"Failed to compile shader \"{name}\".\n---------\nShader info\n---------\n{GL.GetShaderInfoLog(shader)}");

            GL.AttachShader(Id, shader);
        }

        GL.LinkProgram(Id);

        GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out int linkStatus);
        if (linkStatus == 0) throw new ShaderLinkException($"Failed to link shader \"{name}\".\n---------\n{GL.GetProgramInfoLog(Id)}");

        for (int i = 0; i < shaders.Length; i++) {
            int shader = shaders[i];
            GL.DetachShader(Id, shader);
            GL.DeleteShader(shader);
        }
    }



    public void Use() {

        ThrowIfInvalid();

        GL.UseProgram(Id);
    }


    public int GetParam(GetProgramParameterName param) {

        ThrowIfInvalid();

        GL.GetProgram(Id, param, out int value);
        return value;
    }


    // vec1
    public void Uniform(string name, float value) => GL.ProgramUniform1(Id, GetUniformLocation(name), value);
    public void Uniform(string name, int value) => GL.ProgramUniform1(Id, GetUniformLocation(name), value);
    public void Uniform(string name, double value) => GL.ProgramUniform1(Id, GetUniformLocation(name), value);

    // vec2
    public void Uniform(string name, Vec2 value) => GL.ProgramUniform2(Id, GetUniformLocation(name), value);
    public void Uniform(string name, Vec2i value) => GL.ProgramUniform2(Id, GetUniformLocation(name), value);

    // vec3
    public void Uniform(string name, Vec3 value) => GL.ProgramUniform3(Id, GetUniformLocation(name), value);
    public void Uniform(string name, Vec3i value) => GL.ProgramUniform3(Id, GetUniformLocation(name), value);

    // vec4
    public void Uniform(string name, Vec4 value) => GL.ProgramUniform4(Id, GetUniformLocation(name), value);
    public void Uniform(string name, Vec4i value) => GL.ProgramUniform4(Id, GetUniformLocation(name), value);
    public void Uniform(string name, Color4 value) => GL.ProgramUniform4(Id, GetUniformLocation(name), value);

    // mat2
    public void Uniform(string name, Matrix2 value, bool transpose = false) => GL.ProgramUniformMatrix2(Id, GetUniformLocation(name), transpose, ref value);
    public void Uniform(string name, Matrix2d value, bool transpose = false) => GL.ProgramUniformMatrix2(Id, GetUniformLocation(name), transpose, ref value);

    // mat3
    public void Uniform(string name, Matrix3 value, bool transpose = false) => GL.ProgramUniformMatrix3(Id, GetUniformLocation(name), transpose, ref value);
    public void Uniform(string name, Matrix3d value, bool transpose = false) => GL.ProgramUniformMatrix3(Id, GetUniformLocation(name), transpose, ref value);

    // mat4
    public void Uniform(string name, Matrix4 value, bool transpose = false) => GL.ProgramUniformMatrix4(Id, GetUniformLocation(name), transpose, ref value);
    public void Uniform(string name, Matrix4d value, bool transpose = false) => GL.ProgramUniformMatrix4(Id, GetUniformLocation(name), transpose, ref value);



    private int GetUniformLocation(string name) {

        ThrowIfInvalid();

        if (uniformLocations.ContainsKey(name)) {
            return uniformLocations[name];
        } else {
            int location = GL.GetUniformLocation(Id, name);
            if (location >= 0) {
                uniformLocations[name] = location;
                return location;
            }

            throw new ArgumentException($"Could not find the location of uniform \"{name}\" in shader \"{this.name}\".");
        }
    }

    protected override void Delete() {
        GL.DeleteProgram(Id);
    }
}