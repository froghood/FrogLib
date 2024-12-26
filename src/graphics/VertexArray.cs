using OpenTK.Graphics.OpenGL4;

namespace FrogLib;
public class VertexArray {


    public int IndexCount { get; private set; }

    public int Stride { get; }
    public int TotalNumOfComponents { get; }

    private int vbo;
    private int ebo;
    private int vao;

    public unsafe VertexArray(params Layout[] attributes) {

        fixed (int* ptr = &vbo) GL.CreateBuffers(1, ptr);
        fixed (int* ptr = &ebo) GL.CreateBuffers(1, ptr);
        fixed (int* ptr = &vao) GL.CreateVertexArrays(1, ptr);

        Stride = GetStride(attributes);

        GL.VertexArrayVertexBuffer(vao, 0, vbo, 0, Stride);
        GL.VertexArrayElementBuffer(vao, ebo);

        TotalNumOfComponents = GetTotalNumOfComponents(attributes);

        int offset = 0;
        for (int i = 0; i < attributes.Length; i++) {
            var attribute = attributes[i];

            GL.EnableVertexArrayAttrib(vao, i);
            GL.VertexArrayAttribFormat(vao, i, attribute.NumberOfComponents, attribute.Type, false, offset);

            offset += attribute.Size;
        }

        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    }

    public void BufferIndices(int[] data, BufferUsageHint usage) {

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(int), data, usage);

        IndexCount = data.Length;
    }

    public unsafe void BufferVertexData<T>(T[] data, BufferUsageHint usage) where T : unmanaged {
        GL.NamedBufferData(vbo, data.Length * sizeof(T), data, usage);
    }

    public unsafe void BufferVertexSubData<T>(T[] data, int offset, BufferUsageHint usage) where T : unmanaged {
        GL.NamedBufferSubData(vbo, offset, data.Length * sizeof(T), data);
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