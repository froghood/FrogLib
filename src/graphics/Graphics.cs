using System.Collections.Immutable;
using OpenTK.Windowing.Desktop;

namespace FrogLib;

public class Graphics {


    public TextureAtlas SpriteAtlas { get; }
    public TextureLibrary TextureLibrary { get; }
    public ShaderLibrary ShaderLibrary { get; }


    private IGLFWGraphicsContext context;
    private Renderer renderer;

    private List<IRenderable> renderables;


    public Graphics(IGLFWGraphicsContext context) {
        this.context = context;
        renderer = CreateRenderer<DefaultRenderer>();
        renderables = new List<IRenderable>();

        SpriteAtlas = new TextureAtlas();
        TextureLibrary = new TextureLibrary();
        ShaderLibrary = new ShaderLibrary();

    }





    public void Submit(IRenderable renderable) {
        renderables.Add(renderable);
    }

    public void SetRenderer<T>() where T : Renderer {

        renderer.Dispose();

        renderer = CreateRenderer<T>();
        renderer.Init();
    }

    internal void Render() {

        var renderableSnapshot = new IRenderable[renderables.Count];
        renderables.CopyTo(0, renderableSnapshot, 0, renderables.Count);
        renderables.Clear();

        renderer.Render(renderableSnapshot);
    }

    private Renderer CreateRenderer<T>() where T : Renderer {
        return (T)Activator.CreateInstance(typeof(T), new[] { context })!;
    }





}