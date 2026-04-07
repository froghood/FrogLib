using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.OpenGL3;

namespace FrogLib;

public struct RenderableImGuiDrawData : IRenderable {

    private ImDrawDataPtr drawData;

    public RenderableImGuiDrawData(in ImDrawDataPtr drawData) {
        this.drawData = drawData;
    }

    public void Render() {
        ImGuiImplOpenGL3.RenderDrawData(drawData);
    }
}