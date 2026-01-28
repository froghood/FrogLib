using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

internal class ShaderBuilder {

    public int ShaderTypeCount => sources.Length - 1;
    public bool IsCompute => isCompute;
    public bool HasVersion => version != string.Empty;

    private string version = string.Empty;
    private bool isCompute = false;
    private int currentSourceIndex;
    private FastStack<ShaderSource> sources;
    private Dictionary<ShaderType, int> sourceIndices = new();

    public ShaderBuilder() {
        sources = new FastStack<ShaderSource>();
        sources.Push(new ShaderSource(SourceType.Global));
    }

    public void SetVersion(string version) => this.version = version;

    public void SetType(ShaderType type) {
        if (type == ShaderType.ComputeShader) isCompute = true;
        if (!sourceIndices.TryGetValue(type, out currentSourceIndex)) {
            currentSourceIndex = sources.Push(new ShaderSource((SourceType)type));
            sourceIndices[type] = currentSourceIndex;
        }
    }

    public void Write(string text) => sources[currentSourceIndex].Source += text + "\n";

    public Shader Build(string name) {

        var shader = new Shader(name);
        for (int i = 1; i < sources.Length; i++) {
            var global = sources[0];
            var source = sources[i];
            string text = $"#version {version}\n" + global.Source + source.Source;
            shader.Attach((ShaderType)source.Type, text);
        }
        return shader;
    }

    public void Output(string name, string dir) {
        name = name.Replace('\\', '-').Replace('/', '-');
        for (int i = 1; i < sources.Length; i++) {
            var global = sources[0];
            var source = sources[i];
            string text = $"#version {version}\n" + global.Source + source.Source;
            File.WriteAllText(Path.Combine(dir, $"{name}_{source.Type}.glsl"), text);
        }
    }


    private struct ShaderSource {

        public ShaderSource(SourceType type) {
            Type = type;
            Source = "";
        }


        public SourceType Type { get; }
        public string Source;
    }

    private enum SourceType {
        Global = 0,
        Vertex = ShaderType.VertexShader,
        Fragment = ShaderType.FragmentShader,
        Geometry = ShaderType.GeometryShader,
        TessControl = ShaderType.TessControlShader,
        TessEvaluation = ShaderType.TessEvaluationShader,
        Compute = ShaderType.ComputeShader
    }

}