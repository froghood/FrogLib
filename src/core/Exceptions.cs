namespace FrogLib;


public class OpenGLObjectInvalidException : Exception {
    public OpenGLObjectInvalidException(string message) : base(message) { }
}
public class ShaderPreprocessingException : Exception {

    public ShaderPreprocessingException(string message) : base(message) { }
    public ShaderPreprocessingException(string name, string message) : base($"{name}: {message}") { }
    public ShaderPreprocessingException(string name, int line, string message) : base($"{name}({line}): {message}") { }
}

public class ShaderCompilationException : Exception {
    public ShaderCompilationException(string message) : base(message) { }
}

public class ShaderLinkException : Exception {
    public ShaderLinkException(string message) : base(message) { }
}