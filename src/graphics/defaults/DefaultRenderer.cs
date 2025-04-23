
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

public class DefaultRenderer : GameSystem {


    private List<DefaultRenderable> renderables;

    public DefaultRenderer() {
        renderables = new List<DefaultRenderable>();
    }



    public void Submit(DefaultRenderable renderable) => renderables.Add(renderable);



    protected internal override void Startup() {

        Game.Window.FramebufferResize += (e) => GL.Viewport(0, 0, e.Width, e.Height);

        GL.Enable(EnableCap.Blend);

        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

        GL.LoadBindings(new GLFWBindingsContext());

        GL.ProvokingVertex(ProvokingVertexMode.FirstVertexConvention);

        GL.ClearColor(1f, 1f, 1f, 1f);
    }



    protected internal override void Render(float alpha) {

        GL.Clear(ClearBufferMask.ColorBufferBit);

        var sorted = renderables.OrderByDescending(e => e.Depth).ToArray();
        renderables.Clear();

        for (int i = 0; i < sorted.Length; i++) {
            sorted[i].Render();
        }

        Game.Window.Context.SwapBuffers();
    }
}