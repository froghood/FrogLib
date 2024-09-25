
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

public class DefaultRenderer : GameSystem {


    private List<IRenderable> renderables;

    public DefaultRenderer() {
        renderables = new List<IRenderable>();
    }



    public void Submit(IRenderable renderable) => renderables.Add(renderable);



    protected internal override void Startup() {

        GL.Enable(EnableCap.Blend);

        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);

        GL.LoadBindings(new GLFWBindingsContext());

        GL.ProvokingVertex(ProvokingVertexMode.FirstVertexConvention);

        GL.ClearColor(1f, 1f, 1f, 1f);
    }



    protected internal override void Render(float alpha) {

        GL.Clear(ClearBufferMask.ColorBufferBit);

        for (int i = 0; i < renderables.Count; i++) {
            renderables[i].Render();
        }

        renderables.Clear();

        Game.Context.SwapBuffers();
    }
}