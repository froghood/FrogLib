internal static partial class DefaultShaders {

    internal static string RectangleVertexShader {
        get =>
        @"#version 400
        layout(location = 0) in vec2 aPosition;
        flat out vec4 vertexColor;
        uniform mat4 modelMatrix;
        uniform mat4 projectionMatrix;
        uniform vec4 fillColor;
        uniform vec4 strokeColor;
        void main() {       
            vertexColor = gl_VertexID < 4 ? strokeColor : fillColor;       
            gl_Position = projectionMatrix * modelMatrix * vec4(aPosition, 0., 1.);
        }";
    }

    internal static string RectangleFragmentShader {
        get =>
        @"#version 400
        flat in vec4 vertexColor;
        out vec4 color;
        void main() {
            color = vec4(vertexColor.rgb * vertexColor.a, vertexColor.a);
        }";
    }
}
