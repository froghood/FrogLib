using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public struct ShaderSource {

    public ShaderType Type => type;
    public string Source => source;


    private ShaderType type;
    private string source;

    public ShaderSource(ShaderType type, string source) {
        this.type = type;
        this.source = source;
    }

    public static ShaderSource operator +(ShaderSource left, string right) => new ShaderSource(left.Type, left.Source + right);
}