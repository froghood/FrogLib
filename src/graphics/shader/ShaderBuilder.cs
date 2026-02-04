using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

internal class ShaderBuilder {

    public int ShaderTypeCount => sources.Length - 1;
    public bool IsCompute => isCompute;
    public bool HasVersion => version != string.Empty;

    private string version = string.Empty;
    private bool isCompute = false;
    private int currentSourceIndex;


    private bool isGlobal = true;
    private string globalSource = string.Empty;
    private FastStack<ShaderSource> sources = new();
    private FastStack<ShaderSource> buildBuffer = new();
    private Dictionary<ShaderType, int> sourceIndices = new();



    public void SetVersion(string version) => this.version = version;

    public void SetType(ShaderType type) {
        isGlobal = false;
        if (type == ShaderType.ComputeShader) isCompute = true;
        if (!sourceIndices.TryGetValue(type, out currentSourceIndex)) {
            currentSourceIndex = sources.Push(new ShaderSource(type, string.Empty));
            sourceIndices[type] = currentSourceIndex;
        }
    }

    public void Write(string text) {
        if (isGlobal) globalSource += text;
        else sources[currentSourceIndex] += text;
    }

    public Shader Build(string name) {

        buildBuffer.Clear();

        for (int i = 0; i < sources.Length; i++) {
            var source = sources[i];
            buildBuffer.Push(new ShaderSource(
                source.Type,
                $"#version {version}\n" + globalSource + source.Source));
        }

        return new Shader(name, buildBuffer.Span());
    }

    public void Output(string name, string dir) {
        name = name.Replace('\\', '-').Replace('/', '-');
        for (int i = 0; i < sources.Length; i++) {
            var source = sources[i];
            string text = $"#version {version}\n" + globalSource + source.Source;
            File.WriteAllText(Path.Combine(dir, $"{name}_{source.Type}.glsl"), text);
        }
    }
}