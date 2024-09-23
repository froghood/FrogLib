internal static partial class DefaultShaders {

    internal static string CircleVertexShader {
        get =>
        @"#version 400
        layout(location = 0) in vec2 aPosition;
        layout(location = 1) in vec2 aUV;
        uniform mat4 modelMatrix;
        uniform mat4 projectionMatrix;
        out vec2 uv;
        void main() {
            uv = aUV;       
            gl_Position = projectionMatrix * modelMatrix * vec4(aPosition, 0., 1.);
        }";
    }

    internal static string CircleFragmentShader {
        get =>
        @"#version 400
        uniform float radius;
        uniform float outlineWidth;
        uniform vec2 scale;
        uniform vec4 fillColor; 
        uniform vec4 outlineColor;
        in vec2 uv;
        out vec4 outColor;
        void main() { 
            float fragRadius = uv.x * uv.x + uv.y * uv.y;      
            vec2 outline = vec2(1.0) / (radius * scale) * outlineWidth + vec2(1.0);       
            vec2 innerUV = uv * outline;      
            float innerFragRadius = innerUV.x * innerUV.x + innerUV.y * innerUV.y;       
            if (innerFragRadius <= radius * radius) {
                outColor = fillColor;
            } else if (fragRadius <= radius * radius) {
                outColor = outlineColor;
            } else {
                discard;
            }      
            outColor.rgb *= outColor.a;          
        }";
    }
}