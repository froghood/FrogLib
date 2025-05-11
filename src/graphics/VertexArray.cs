using OpenTK.Graphics.OpenGL4;

namespace FrogLib;
public class VertexArray {


    public Buffer VertexBuffer => vertexBuffer;
    public Buffer IndexBuffer => indexBuffer;


    public int Stride { get; }
    public int TotalNumOfComponents { get; }

    private Buffer vertexBuffer;
    private Buffer indexBuffer;
    private int vao;

    public unsafe VertexArray(int vertexSize, int indexSize, params Layout[] attributes) {

        fixed (int* ptr = &vao) GL.CreateVertexArrays(1, ptr);

        vertexBuffer = new Buffer();
        indexBuffer = new Buffer();
        vertexBuffer.BufferStorage(vertexSize, BufferStorageFlags.DynamicStorageBit);
        indexBuffer.BufferStorage(indexSize, BufferStorageFlags.DynamicStorageBit);

        Stride = GetStride(attributes);
        TotalNumOfComponents = GetTotalNumOfComponents(attributes);

        GL.VertexArrayVertexBuffer(vao, 0, vertexBuffer.Handle, 0, Stride);
        GL.VertexArrayElementBuffer(vao, indexBuffer.Handle);


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