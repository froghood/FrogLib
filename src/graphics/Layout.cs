using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public struct Layout {

    public VertexAttribPointerType Type { get; }
    public int NumberOfComponents { get; }
    public int TypeSize { get; }
    public int Size { get; }

    public Layout(VertexAttribPointerType type, int numberOfComponents) {

        Type = type;
        NumberOfComponents = numberOfComponents;

        TypeSize = Type switch {
            VertexAttribPointerType.Byte => sizeof(byte),
            VertexAttribPointerType.Short => sizeof(short),
            VertexAttribPointerType.Int => sizeof(int),
            VertexAttribPointerType.Float => sizeof(float),
            VertexAttribPointerType.Double => sizeof(double),
            _ => throw new Exception("Vertex attribute pointer type size not implemented.")
        };

        Size = TypeSize * NumberOfComponents;
    }
}