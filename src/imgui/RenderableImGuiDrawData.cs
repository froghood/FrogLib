using ImGuiNET;

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