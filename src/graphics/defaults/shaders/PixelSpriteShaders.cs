internal static partial class DefaultShaders {

    internal static string PixelSpriteVertexShader {
        get =>
        @"#version 400
        layout(location = 0) in vec2 aPosition;
        layout(location = 1) in vec2 aTexel;
        uniform mat4 modelMatrix;
        uniform mat4 projectionMatrix;
        out vec2 texel;
        void main() {
            texel = aTexel;    
            gl_Position = (projectionMatrix * modelMatrix * vec4(aPosition.xy, 0., 1.));
        }";
    }

    internal static string PixelSpriteFragmentShader {
        get =>
        @"#version 400
        in vec2 texel;
        out vec4 outColor;
        uniform sampler2D texture0;
        uniform vec2 scale;
        uniform vec2 spriteTopLeft;
        uniform vec2 spriteBottomRight;
        uniform vec2 textureSize;
        uniform vec4 color;
        vec4 sampleTexture(vec2 texel) {           
            if (texel.x < spriteTopLeft.x || texel.x >= spriteBottomRight.x ||
                texel.y < spriteTopLeft.y || texel.y >= spriteBottomRight.y) {
                    return vec4(0, 0, 0, 0);
                }          
            return texture(texture0, (texel + 0.5) / textureSize);
        }
        
        vec4 sampleBilinear(vec2 texel) {         
            texel -= 0.5;        
            vec2 nearestTopLeft = floor(texel);
            vec2 nearestBottomRight = floor(texel) + 1;        
            float topBias = 1. - (texel.y - nearestTopLeft.y);
            float leftBias = 1. - (texel.x - nearestTopLeft.x);         
            float bottomBias = 1. - (nearestBottomRight.y - texel.y);
            float rightBias = 1. - (nearestBottomRight.x - texel.x);        
            vec4 topLeftSample = sampleTexture(nearestTopLeft) * topBias * leftBias;
            vec4 bottomRightSample = sampleTexture(nearestBottomRight) * bottomBias * rightBias;       
            vec4 topRightSample = sampleTexture(vec2(nearestBottomRight.x, nearestTopLeft.y)) * topBias * rightBias;
            vec4 bottomLeftSample = sampleTexture(vec2(nearestTopLeft.x, nearestBottomRight.y)) * bottomBias * leftBias;
            return topLeftSample + bottomRightSample + topRightSample + bottomLeftSample;
        }

        vec2 edgeFunction(vec2 n) {           
            vec2 _scale = max(scale, 1);
            return max(min(n * _scale, 0.5), n * _scale - _scale + 1);   
        }

        void main() {          
            vec2 edge = edgeFunction(fract(texel));          
            vec2 newTexel = floor(texel) + edge;          
            vec4 textureColor = sampleBilinear(newTexel) * color;
            textureColor.rgb * color.a;        
            outColor = textureColor;   
        }";
    }
}