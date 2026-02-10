using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

internal static class ShaderReader {

    private static Dictionary<string, DirectiveAction> directiveActions = new(){
        {"version", VersionDirective},
        {"include", IncludeDirective},
        {"vertex", (builder, name, line, args) => TypeDirective(ShaderType.VertexShader, builder, name, line, args)},
        {"fragment", (builder, name, line, args) => TypeDirective(ShaderType.FragmentShader, builder, name, line, args)},
        {"geometry", (builder, name, line, args) => TypeDirective(ShaderType.GeometryShader, builder, name, line, args)},
        {"tesscontrol", (builder, name, line, args) => TypeDirective(ShaderType.TessControlShader, builder, name, line, args)},
        {"tessevaluation", (builder, name, line, args) => TypeDirective(ShaderType.TessEvaluationShader, builder, name, line, args)},
        {"compute", (builder, name, line, args) => TypeDirective(ShaderType.ComputeShader, builder, name, line, args)},
    };



    public static Shader Read(TextReader text, string path, string name, string outputDir) {
        bool isFile = path != string.Empty;
        string sourceName = isFile ? PathExt.RemoveRelativePath(PathExt.ToUnixPath(path)) : "String Source";

        var builder = new ShaderBuilder();

        int lineNumber = 0;
        bool writeLine = true;

        while (true) {

            var line = text.ReadLine();
            if (line == null) break;

            lineNumber++;

            if (string.IsNullOrWhiteSpace(line)) {
                writeLine = true;
                continue;
            }

            line = line.Trim();

            if (!line.StartsWith('%')) {
                if (writeLine) {
                    builder.Write($"#line {lineNumber} \"{sourceName}\"\n");
                    writeLine = false;
                }
                builder.Write(line + '\n');
                continue;
            }

            var tokens = line[1..].Split(null);
            var directive = tokens[0].ToLower();

            if (!directiveActions.TryGetValue(directive, out var action)) {
                throw new ShaderPreprocessingException(sourceName, lineNumber, $"Unknown directive: {directive}");
            }

            var args = tokens.Length > 1 ? string.Join(' ', tokens[1..]).Trim() : string.Empty;

            action.Invoke(builder, sourceName, lineNumber, args);

        }

        if (!builder.HasVersion) throw new ShaderPreprocessingException(sourceName, "No version directive found, e.g. %version 330");
        if (builder.ShaderTypeCount == 0) throw new ShaderPreprocessingException(sourceName, "Shader type could not be inferred. Be sure to include one or more shader type directives in your shader source file, e.g. %vertex");
        if (builder.IsCompute && builder.ShaderTypeCount > 1) throw new ShaderPreprocessingException(sourceName, "Compute directives are incompatible with other shader types.");


        if (outputDir != string.Empty) builder.Output(name, outputDir);

        return builder.Build(name);

    }


    private static void VersionDirective(ShaderBuilder builder, string name, int line, string args) {
        if (builder.HasVersion) throw new ShaderPreprocessingException(name, line, "Repeated version directive.");
        builder.SetVersion(args);
    }

    private static void IncludeDirective(ShaderBuilder builder, string fileName, int lineNumber, string path) {

        using var file = File.OpenText(path);

        path = PathExt.RemoveRelativePath(PathExt.ToUnixPath(path));

        int includeLine = 0;
        bool writeLine = true;



        while (true) {

            var line = file.ReadLine();
            if (line == null) break;

            includeLine++;

            if (string.IsNullOrWhiteSpace(line)) {
                writeLine = true;
                continue;
            }

            line = line.Trim();

            if (writeLine) {
                builder.Write($"#line {includeLine} \"{path}\"\n");
                writeLine = false;
            }
            builder.Write(line + '\n');
        }
    }

    public static void TypeDirective(ShaderType type, ShaderBuilder builder, string fileName, int line, string args) {
        if (args != string.Empty) throw new ShaderPreprocessingException(fileName, line, "Unexpected argument after shader type directive.");

        builder.SetType(type);
    }

    private delegate void DirectiveAction(ShaderBuilder builder, string name, int line, string args);
}








