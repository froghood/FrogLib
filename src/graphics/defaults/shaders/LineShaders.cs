internal static partial class DefaultShaders {

    internal static string LineVertexShader {
        get =>
        @"#version 400
        layout(location = 0) in vec2 aPosition;
        out vec4 vertexColor;
        uniform mat4 modelMatrix;
        uniform mat4 projectionMatrix;
        uniform vec4 color;
        void main() {        
            vertexColor = color;       
            gl_Position = projectionMatrix * modelMatrix * vec4(aPosition, 0., 1.);
        }";
    }

    internal static string LineFragmentShader {
        get =>
        @"#version 400
        in vec4 vertexColor;
        out vec4 color;
        void main() {
            color = vec4(vertexColor.rgb * vertexColor.a, vertexColor.a);
        }";
    }
}