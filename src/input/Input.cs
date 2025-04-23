using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

/// <summary>
/// wrapper around opentk Game.Window input
/// </summary>
public class Input : IGameSystem {

    public event Action<KeyboardKeyEventArgs>? KeyDown;
    public event Action<KeyboardKeyEventArgs>? KeyUp;
    public event Action<TextInputEventArgs>? TextInput;
    public event Action<MouseButtonEventArgs>? MouseDown;
    public event Action<MouseButtonEventArgs>? MouseUp;
    public event Action<MouseWheelEventArgs>? MouseWheel;
    public event Action<MouseMoveEventArgs>? MouseMove;



    protected internal override void Startup() {
        Game.Window.KeyDown += (e) => KeyDown?.Invoke(e);
        Game.Window.KeyUp += (e) => KeyUp?.Invoke(e);
        Game.Window.TextInput += (e) => TextInput?.Invoke(e);
        Game.Window.MouseDown += (e) => MouseDown?.Invoke(e);
        Game.Window.MouseUp += (e) => MouseUp?.Invoke(e);
        Game.Window.MouseWheel += (e) => MouseWheel?.Invoke(e);
        Game.Window.MouseMove += (e) => MouseMove?.Invoke(e);
    }

    protected internal override void Update() {
        Game.Window.ProcessEvents(0);
    }



    public bool IsKeyDown(Keys key) => Game.Window.IsKeyDown(key);
    public bool IsKeyPressed(Keys key) => Game.Window.IsKeyPressed(key);
    public bool IsKeyReleased(Keys key) => Game.Window.IsKeyReleased(key);
    public bool IsMouseDown(MouseButton button) => Game.Window.IsMouseButtonDown(button);
    public bool IsMousePressed(MouseButton button) => Game.Window.IsMouseButtonPressed(button);
    public bool IsMouseReleased(MouseButton button) => Game.Window.IsMouseButtonReleased(button);



    public Vector2 GetMousePosition() => Game.Window.MousePosition;
    public Vector2 GetMouseDelta() => Game.Window.MouseState.Delta;
    public void SetMousePosition(Vector2 position) => Game.Window.MousePosition = position;
    public Vector2 GetMouseScroll() => Game.Window.MouseState.ScrollDelta;



    public MouseState GetMouseState() => Game.Window.MouseState;
    public KeyboardState GetKeyboardState() => Game.Window.KeyboardState;



}