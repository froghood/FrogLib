using Hexa.NET.ImGui;

using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Backends.OpenGL3;
using OpenTK.Graphics.OpenGL4;


namespace FrogLib;

public unsafe class ImGuiController : Module {

    protected internal override void Startup() {

        var context = ImGui.CreateContext();

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

        var window = new GLFWwindowPtr((GLFWwindow*)Game.Window.WindowPtr);

        ImGuiImplGLFW.SetCurrentContext(context);
        ImGuiImplGLFW.InitForOpenGL(window, true);

        ImGuiImplOpenGL3.SetCurrentContext(context);

        int major = GL.GetInteger(GetPName.MajorVersion);
        int minor = GL.GetInteger(GetPName.MinorVersion);
        string version = $"#version {major * 100 + minor * 10}";

        ImGuiImplOpenGL3.Init(version);
    }

    public RenderableImGuiDrawData NewFrame(Action onFrame) {

        ImGuiImplOpenGL3.NewFrame();
        ImGuiImplGLFW.NewFrame();
        ImGui.NewFrame();

        onFrame?.Invoke();

        ImGui.Render();

        if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable)) {
            ImGui.UpdatePlatformWindows();
            ImGui.RenderPlatformWindowsDefault();
            Game.Window.Context.MakeCurrent();
        }

        return new RenderableImGuiDrawData(ImGui.GetDrawData());
    }
}