using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FrogLib;

public class Shader {

    public int Handle => program;
    private int program;



    private Dictionary<string, int> uniformLocations;



    public Shader() {
        program = GL.CreateProgram();
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
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0) {
            string infoLog = GL.GetProgramInfoLog(program);
            Log.Warn(infoLog);
        }
        return this;
    }



    public void Use() => GL.UseProgram(program);



    // vec1
    public void Uniform(string name, float value) => GL.ProgramUniform1(program, GetUniformLocation(name), value);
    public void Uniform(string name, int value) => GL.ProgramUniform1(program, GetUniformLocation(name), value);
    public void Uniform(string name, double value) => GL.ProgramUniform1(program, GetUniformLocation(name), value);

    // vec2
    public void Uniform(string name, Vector2 value) => GL.ProgramUniform2(program, GetUniformLocation(name), value);
    public void Uniform(string name, Vector2i value) => GL.ProgramUniform2(program, GetUniformLocation(name), value);

    // vec3
    public void Uniform(string name, Vector3 value) => GL.ProgramUniform3(program, GetUniformLocation(name), value);
    public void Uniform(string name, Vector3i value) => GL.ProgramUniform3(program, GetUniformLocation(name), value);

    // vec4
    public void Uniform(string name, Vector4 value) => GL.ProgramUniform4(program, GetUniformLocation(name), value);
    public void Uniform(string name, Vector4i value) => GL.ProgramUniform4(program, GetUniformLocation(name), value);
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
}