using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

public class Input {

    public event Action<KeyboardKeyEventArgs>? KeyDown;
    public event Action<KeyboardKeyEventArgs>? KeyUp;
    public event Action<TextInputEventArgs>? TextInput;

    private NativeWindow window;


    internal Input(NativeWindow window) {
        this.window = window;

        this.window.KeyDown += (e) => KeyDown?.Invoke(e);
        this.window.KeyUp += (e) => KeyUp?.Invoke(e);
        this.window.TextInput += (e) => TextInput?.Invoke(e);
    }



    public bool IsKeyDown(Keys key) => window.IsKeyDown(key);
    public bool IsKeyPressed(Keys key) => window.IsKeyPressed(key);
    public bool IsKeyReleased(Keys key) => window.IsKeyReleased(key);
    public bool IsMouseDown(MouseButton button) => window.IsMouseButtonDown(button);
    public bool IsMousePressed(MouseButton button) => window.IsMouseButtonPressed(button);
    public bool IsMouseReleased(MouseButton button) => window.IsMouseButtonReleased(button);
    public Vector2 GetMousePosition() => window.MousePosition;
    public void SetMousePosition(Vector2 position) => window.MousePosition = position;
    public Vector2 GetMouseScroll() => window.MouseState.ScrollDelta;
    public Vector2 GetMouseDelta() => window.MouseState.Delta;
    public MouseState GetMouseState() => window.MouseState;
    public KeyboardState GetKeyboardState() => window.KeyboardState;



    internal void PollInputs() {
        window.ProcessEvents(0);
    }
}