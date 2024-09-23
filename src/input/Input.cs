using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

public class Input {

    private NativeWindow window;

    internal Input(NativeWindow window) {
        this.window = window;
    }



    public bool IsKeyDown(Keys key) => window.IsKeyDown(key);
    public bool IsKeyPressed(Keys key) => window.IsKeyPressed(key);
    public bool IsKeyReleased(Keys key) => window.IsKeyReleased(key);
    public bool IsMouseDown(MouseButton button) => window.IsMouseButtonDown(button);
    public bool IsMousePressed(MouseButton button) => window.IsMouseButtonPressed(button);
    public bool IsMouseReleased(MouseButton button) => window.IsMouseButtonReleased(button);


    public Vector2 GetMousePosition() => window.MousePosition;
    public Vector2 GetMouseScroll() => window.MouseState.ScrollDelta;





    internal void PollInputs() {
        window.ProcessEvents(0);
    }
}