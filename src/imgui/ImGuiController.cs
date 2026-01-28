using ImGuiNET;
using OpenTK.Graphics.ES11;

namespace FrogLib;

public class ImGuiController : Module {

    protected internal override void Startup() {

        ImGui.CreateContext();
        var io = ImGui.GetIO();

        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

        ImGuiImplOpenTK4.Init(Game.Window);
        ImGuiImplOpenGL3.Init();
    }

    public RenderableImGuiDrawData NewFrame(Action onFrame) {

        ImGuiImplOpenGL3.NewFrame();
        ImGuiImplOpenTK4.NewFrame();
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