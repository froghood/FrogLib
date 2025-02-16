using OpenTK.Graphics.OpenGL4;

namespace FrogLib;
public class VertexArray {

    public int Stride { get; }
    public int TotalNumOfComponents { get; }

    private int vbo;
    private int ibo;
    private int vao;

    public unsafe VertexArray(int vertexSize, int indexSize, params Layout[] attributes) {

        fixed (int* ptr = &vbo) GL.CreateBuffers(1, ptr);
        fixed (int* ptr = &ibo) GL.CreateBuffers(1, ptr);
        fixed (int* ptr = &vao) GL.CreateVertexArrays(1, ptr);

        GL.NamedBufferStorage(vbo, vertexSize, 0, BufferStorageFlags.DynamicStorageBit);
        GL.NamedBufferStorage(ibo, indexSize, 0, BufferStorageFlags.DynamicStorageBit);

        Stride = GetStride(attributes);

        GL.VertexArrayVertexBuffer(vao, 0, vbo, 0, Stride);
        GL.VertexArrayElementBuffer(vao, ibo);

        TotalNumOfComponents = GetTotalNumOfComponents(attributes);

        int offset = 0;
        for (int i = 0; i < attributes.Length; i++) {
            var attribute = attributes[i];

            GL.EnableVertexArrayAttrib(vao, i);

            switch (attribute.Format) {
                case LayoutFormat.Float:
                    GL.VertexArrayAttribFormat(vao, i, attribute.NumberOfComponents, attribute.Type, false, offset);
                    break;
                case LayoutFormat.Int:
                    GL.VertexArrayAttribIFormat(vao, i, attribute.NumberOfComponents, attribute.Type, offset);
                    break;
                case LayoutFormat.Double:
                    GL.VertexArrayAttribLFormat(vao, i, attribute.NumberOfComponents, attribute.Type, offset);
                    break;
            }

            GL.VertexArrayAttribBinding(vao, i, 0);

            offset += attribute.Size;
        }
    }



    public unsafe void BufferVertices<T>(T[] vertices) where T : unmanaged {
        GL.NamedBufferSubData(vbo, 0, vertices.Length * sizeof(T), vertices);
    }

    public unsafe void BufferVertices<T>(T[] vertices, int offset) where T : unmanaged {
        GL.NamedBufferSubData(vbo, offset, vertices.Length * sizeof(T), vertices);
    }

    public unsafe void BufferVertices<T>(T[] vertices, int offset, int size) where T : unmanaged {
        GL.NamedBufferSubData(vbo, offset, size, vertices);
    }

    public unsafe void BufferVertices<T>(ref ReadOnlySpan<T> vertices) where T : unmanaged {
        fixed (T* ptr = &vertices[0])
            GL.NamedBufferSubData(vbo, 0, vertices.Length * sizeof(T), (nint)ptr);
    }

    public unsafe void BufferVertices<T>(ref ReadOnlySpan<T> vertices, int offset) where T : unmanaged {
        fixed (T* ptr = &vertices[0])
            GL.NamedBufferSubData(vbo, offset, vertices.Length * sizeof(T), (nint)ptr);
    }

    public unsafe void BufferVertices<T>(ref ReadOnlySpan<T> vertices, int offset, int size) where T : unmanaged {
        fixed (T* ptr = &vertices[0])
            GL.NamedBufferSubData(vbo, offset, size, (nint)ptr);
    }

    public unsafe void BufferVertices<T>(ref T* vertices, int size) where T : unmanaged {
        GL.NamedBufferSubData(vbo, 0, size, (nint)vertices);
    }

    public unsafe void BufferVertices<T>(ref T* vertices, int offset, int size) where T : unmanaged {
        GL.NamedBufferSubData(vbo, offset, size, (nint)vertices);
    }





    public unsafe void BufferIndices<T>(T[] indices) where T : unmanaged {
        GL.NamedBufferSubData(ibo, 0, indices.Length * sizeof(T), indices);
    }

    public unsafe void BufferIndices<T>(T[] indices, int offset) where T : unmanaged {
        GL.NamedBufferSubData(ibo, offset, indices.Length * sizeof(T), indices);
    }

    public unsafe void BufferIndices<T>(T[] indices, int offset, int size) where T : unmanaged {
        GL.NamedBufferSubData(ibo, offset, size, indices);
    }

    public unsafe void BufferIndices<T>(ref ReadOnlySpan<T> indices) where T : unmanaged {
        fixed (T* ptr = &indices[0])
            GL.NamedBufferSubData(ibo, 0, indices.Length * sizeof(T), (nint)ptr);
    }

    public unsafe void BufferIndices<T>(ref ReadOnlySpan<T> indices, int offset) where T : unmanaged {
        fixed (T* ptr = &indices[0])
            GL.NamedBufferSubData(ibo, offset, indices.Length * sizeof(T), (nint)ptr);
    }

    public unsafe void BufferIndices<T>(ref ReadOnlySpan<T> indices, int offset, int size) where T : unmanaged {
        fixed (T* ptr = &indices[0])
            GL.NamedBufferSubData(ibo, offset, size, (nint)ptr);
    }

    public unsafe void BufferIndices<T>(ref T* indices, int size) where T : unmanaged {
        GL.NamedBufferSubData(ibo, 0, size, (nint)indices);
    }

    public unsafe void BufferIndices<T>(ref T* indices, int offset, int size) where T : unmanaged {
        GL.NamedBufferSubData(ibo, offset, size, (nint)indices);
    }



    public void Use() => GL.BindVertexArray(vao);



    private static int GetStride(Layout[] attributes) {
        int stride = 0;
        for (int i = 0; i < attributes.Length; i++) {
            stride += attributes[i].Size;
        }
        return stride;
    }

    private static int GetTotalNumOfComponents(Layout[] attributes) {
        int totalNumOfComponents = 0;
        for (int i = 0; i < attributes.Length; i++) {
            totalNumOfComponents += attributes[i].NumberOfComponents;
        }
        return totalNumOfComponents;
    }
}