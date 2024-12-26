using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public struct Layout {

    public VertexAttribType Type { get; }
    public int NumberOfComponents { get; }
    public int TypeSize { get; }
    public int Size { get; }

    public Layout(VertexAttribType type, int numberOfComponents) {

        Type = type;
        NumberOfComponents = numberOfComponents;

        TypeSize = Type switch {
            VertexAttribType.Byte or
            VertexAttribType.UnsignedByte
            => 1,

            VertexAttribType.Short or
            VertexAttribType.UnsignedShort or
            VertexAttribType.HalfFloat
            => 2,

            VertexAttribType.Int or
            VertexAttribType.UnsignedInt or
            VertexAttribType.Float
            => 4,

            VertexAttribType.Double => 8,

            _ => throw new Exception("Vertex attribute pointer type size not implemented.")
        };

        Size = TypeSize * NumberOfComponents;
    }
}