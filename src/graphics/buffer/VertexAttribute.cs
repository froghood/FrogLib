using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public struct VertexAttribute {

    public VertexAttribType Type { get; }
    public int NumberOfComponents { get; }
    public int TypeSize { get; }
    public int Size { get; }
    internal FormatType Format { get; }

    public VertexAttribute(VertexAttribType type, int numberOfComponents) {

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

            VertexAttribType.Double
            => 8,

            _ => throw new NotImplementedException("Vertex attribute pointer type size not implemented.")
        };

        Size = TypeSize * NumberOfComponents;

        Format = Type switch {
            VertexAttribType.Byte or
            VertexAttribType.UnsignedByte or
            VertexAttribType.Short or
            VertexAttribType.UnsignedShort or
            VertexAttribType.Int or
            VertexAttribType.UnsignedInt
            => FormatType.Int,

            VertexAttribType.HalfFloat or
            VertexAttribType.Float
            => FormatType.Float,

            VertexAttribType.Double
            => FormatType.Double,

            _ => throw new NotImplementedException("Vertex attribute pointer type not implemented.")
        };
    }

    public enum FormatType { Float, Int, Double };
}