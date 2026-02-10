using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public class VertexArray : GLObject {

    public Buffer VertexBuffer => vertexBuffer;
    public Buffer IndexBuffer => indexBuffer;

    public int Stride { get; }
    public int TotalNumOfComponents { get; }



    private Buffer vertexBuffer;
    private Buffer indexBuffer;




    public VertexArray(int vertexSize, int indexSize, params VertexAttribute[] attributes)
    : base(GL.CreateVertexArray()) {

        vertexBuffer = new Buffer();
        indexBuffer = new Buffer();
        vertexBuffer.BufferStorage(vertexSize, BufferStorageFlags.DynamicStorageBit);
        indexBuffer.BufferStorage(indexSize, BufferStorageFlags.DynamicStorageBit);

        Stride = GetStride(attributes);
        TotalNumOfComponents = GetTotalNumOfComponents(attributes);

        GL.VertexArrayVertexBuffer(Handle, 0, vertexBuffer.Handle, 0, Stride);
        GL.VertexArrayElementBuffer(Handle, indexBuffer.Handle);


        int offset = 0;
        for (int i = 0; i < attributes.Length; i++) {
            var attribute = attributes[i];

            GL.EnableVertexArrayAttrib(Handle, i);

            switch (attribute.Format) {
                case VertexAttribute.FormatType.Float:
                    GL.VertexArrayAttribFormat(Handle, i, attribute.NumberOfComponents, attribute.Type, false, offset);
                    break;
                case VertexAttribute.FormatType.Int:
                    GL.VertexArrayAttribIFormat(Handle, i, attribute.NumberOfComponents, attribute.Type, offset);
                    break;
                case VertexAttribute.FormatType.Double:
                    GL.VertexArrayAttribLFormat(Handle, i, attribute.NumberOfComponents, attribute.Type, offset);
                    break;
            }

            GL.VertexArrayAttribBinding(Handle, i, 0);

            offset += attribute.Size;
        }
    }



    public void Use() => GL.BindVertexArray(Handle);



    private static int GetStride(VertexAttribute[] attributes) {
        int stride = 0;
        for (int i = 0; i < attributes.Length; i++) {
            stride += attributes[i].Size;
        }
        return stride;
    }

    private static int GetTotalNumOfComponents(VertexAttribute[] attributes) {
        int totalNumOfComponents = 0;
        for (int i = 0; i < attributes.Length; i++) {
            totalNumOfComponents += attributes[i].NumberOfComponents;
        }
        return totalNumOfComponents;
    }

    protected override void Delete() {
        vertexBuffer.Dispose();
        indexBuffer.Dispose();
        GL.DeleteVertexArray(Handle);
    }
}