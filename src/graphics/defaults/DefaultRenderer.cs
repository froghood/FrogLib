
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

public class DefaultRenderer : Renderer {


    internal DefaultRenderer(IGLFWGraphicsContext context) : base(context) { }

    public override void Init() {

        GL.Enable(EnableCap.Blend);

        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

        GL.LoadBindings(new GLFWBindingsContext());

        GL.ProvokingVertex(ProvokingVertexMode.FirstVertexConvention);

        GL.ClearColor(1f, 1f, 1f, 1f);
    }

    public override void Render(IRenderable[] renderables) {

        GL.Clear(ClearBufferMask.ColorBufferBit);

        for (int i = 0; i < renderables.Length; i++) {
            var renderable = renderables[i];

            renderable.Render();
        }

        Context.SwapBuffers();
    }
}