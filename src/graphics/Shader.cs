using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class Shader : IDisposable {

    public int Handle => program;
    private int program;
    private string name;



    private Dictionary<string, int> uniformLocations;



    public Shader(string name) {

        program = GL.CreateProgram();
        this.name = name;
        uniformLocations = new Dictionary<string, int>();
    }



    public Shader FromPath(ShaderType type, string path) {
        var source = File.ReadAllText(path);
        return FromSource(type, source);
    }

    public Shader FromSource(ShaderType type, string source) {
        var shader = GL.CreateShader(type);

        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.AttachShader(program, shader);
        GL.DeleteShader(shader);
        GL.LinkProgram(program);

        return this;
    }



    public void Use() => GL.UseProgram(program);


    public int GetParam(GetProgramParameterName param) {
        GL.GetProgram(program, param, out int value);
        return value;
    }



    // vec1
    public void Uniform(string name, float value) => GL.ProgramUniform1(program, GetUniformLocation(name), value);
    public void Uniform(string name, int value) => GL.ProgramUniform1(program, GetUniformLocation(name), value);
    public void Uniform(string name, double value) => GL.ProgramUniform1(program, GetUniformLocation(name), value);

    // vec2
    public void Uniform(string name, Vec2 value) => GL.ProgramUniform2(program, GetUniformLocation(name), value);
    public void Uniform(string name, Vec2i value) => GL.ProgramUniform2(program, GetUniformLocation(name), value);

    // vec3
    public void Uniform(string name, Vec3 value) => GL.ProgramUniform3(program, GetUniformLocation(name), value);
    public void Uniform(string name, Vec3i value) => GL.ProgramUniform3(program, GetUniformLocation(name), value);

    // vec4
    public void Uniform(string name, Vec4 value) => GL.ProgramUniform4(program, GetUniformLocation(name), value);
    public void Uniform(string name, Vec4i value) => GL.ProgramUniform4(program, GetUniformLocation(name), value);
    public void Uniform(string name, Color4 value) => GL.ProgramUniform4(program, GetUniformLocation(name), value);

    // mat2
    public void Uniform(string name, Matrix2 value, bool transpose = false) => GL.ProgramUniformMatrix2(program, GetUniformLocation(name), transpose, ref value);
    public void Uniform(string name, Matrix2d value, bool transpose = false) => GL.ProgramUniformMatrix2(program, GetUniformLocation(name), transpose, ref value);

    // mat3
    public void Uniform(string name, Matrix3 value, bool transpose = false) => GL.ProgramUniformMatrix3(program, GetUniformLocation(name), transpose, ref value);
    public void Uniform(string name, Matrix3d value, bool transpose = false) => GL.ProgramUniformMatrix3(program, GetUniformLocation(name), transpose, ref value);

    // mat4
    public void Uniform(string name, Matrix4 value, bool transpose = false) => GL.ProgramUniformMatrix4(program, GetUniformLocation(name), transpose, ref value);
    public void Uniform(string name, Matrix4d value, bool transpose = false) => GL.ProgramUniformMatrix4(program, GetUniformLocation(name), transpose, ref value);



    private int GetUniformLocation(string name) {
        if (uniformLocations.ContainsKey(name)) {
            return uniformLocations[name];
        } else {
            int location = GL.GetUniformLocation(program, name);
            if (location >= 0) {
                uniformLocations[name] = location;
                return location;
            }

            throw new Exception($"Could not find the location of uniform {name}");
        }
    }

    public void Dispose() => GL.DeleteProgram(program);
}