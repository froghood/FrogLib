using OpenTK.Graphics.OpenGL4;

namespace FrogLib;
public class VertexArray {


    public int IndexCount { get; private set; }

    public int Stride { get; }
    public int TotalNumOfComponents { get; }

    private int vao;
    private int vbo;
    private int ibo;

    public VertexArray(params Layout[] attributes) {
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        ibo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        Stride = attributes.Sum(e => e.Size);
        TotalNumOfComponents = attributes.Sum(e => e.NumberOfComponents);

        int offset = 0;
        for (uint i = 0; i < attributes.Length; i++) {
            var attribute = attributes[i];

            GL.VertexAttribPointer(i, attribute.NumberOfComponents, attribute.Type, false, Stride, offset);
            GL.EnableVertexAttribArray(i);

            offset += attribute.Size;
        }

        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    }

    public void BufferIndices(int[] data, BufferUsageHint usage) {

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(int), data, usage);

        IndexCount = data.Length;
    }

    public void BufferVertexData(float[] data, BufferUsageHint usage) {

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, usage);
    }

    public void Bind() => GL.BindVertexArray(vao);
}