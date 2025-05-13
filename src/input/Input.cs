using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace FrogLib;

/// <summary>
/// wrapper around opentk Game.Window input
/// </summary>
public class Input : GameSystem {



    private InputLoopState loopState;



    private MouseState updateMouseState = new();
    private MouseState renderMouseState = new();

    private KeyboardState updateKeyboardState = new();
    private KeyboardState renderKeyboardState = new();



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



    protected internal override void PreUpdate() {
        loopState = InputLoopState.Update;

        updateMouseState.Set(Game.Window.MouseState);
        updateKeyboardState.Set(Game.Window.KeyboardState);
    }

    protected internal override void PreRender(float alpha) {
        loopState = InputLoopState.Render;

        renderMouseState.Set(Game.Window.MouseState);
        renderKeyboardState.Set(Game.Window.KeyboardState);
    }



    public bool IsKeyDown(Keys key) {
        return loopState switch {
            InputLoopState.Update => updateKeyboardState.IsKeyDown(key),
            InputLoopState.Render => renderKeyboardState.IsKeyDown(key),
            _ => false
        };
    }



    /// <summary>
    /// returns true if the key is down this frame but not the last frame
    /// if called during an update frame, it's relative to the previous update frame
    /// if called during a render frame, it's relative to the previous render frame
    /// </summary>
    public bool IsKeyPressed(Keys key) {
        return loopState switch {
            InputLoopState.Update => updateKeyboardState.IsKeyPressed(key),
            InputLoopState.Render => renderKeyboardState.IsKeyPressed(key),
            _ => false
        };
    }



    /// <summary>
    /// returns true if the key is up this frame but not the last frame
    /// if called during an update frame, it's relative to the previous update frame
    /// if called during a render frame, it's relative to the previous render frame
    /// </summary>
    public bool IsKeyReleased(Keys key) {
        return loopState switch {
            InputLoopState.Update => updateKeyboardState.IsKeyReleased(key),
            InputLoopState.Render => renderKeyboardState.IsKeyReleased(key),
            _ => false
        };
    }

    public bool IsMouseDown(MouseButton button) {
        return loopState switch {
            InputLoopState.Update => updateMouseState.IsButtonDown(button),
            InputLoopState.Render => renderMouseState.IsButtonDown(button),
            _ => false
        };
    }



    /// <summary>
    /// returns true if the mouse button is down this frame but not the last frame
    /// if called during an update frame, it's relative to the previous update frame
    /// if called during a render frame, it's relative to the previous render frame
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool IsMousePressed(MouseButton button) {
        return loopState switch {
            InputLoopState.Update => updateMouseState.IsButtonPressed(button),
            InputLoopState.Render => renderMouseState.IsButtonPressed(button),
            _ => false
        };
    }



    /// <summary>
    /// returns true if the mouse button is up this frame but not the last frame
    /// if called during an update frame, it's relative to the previous update frame
    /// if called during a render frame, it's relative to the previous render frame
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool IsMouseReleased(MouseButton button) {
        return loopState switch {
            InputLoopState.Update => updateMouseState.IsButtonReleased(button),
            InputLoopState.Render => renderMouseState.IsButtonReleased(button),
            _ => false
        };
    }



    public Vector2 GetMousePosition() {
        return loopState switch {
            InputLoopState.Update => updateMouseState.Position,
            InputLoopState.Render => renderMouseState.Position,
            _ => Vector2.Zero
        };
    }



    /// <summary>
    /// returns the change in mouse position since the last frame
    /// if called during an update frame, it's relative to the previous update frame
    /// if called during a render frame, it's relative to the previous render frame
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMousePositionDelta() {
        return loopState switch {
            InputLoopState.Update => updateMouseState.PositionDelta,
            InputLoopState.Render => renderMouseState.PositionDelta,
            _ => Vector2.Zero
        };
    }



    public void SetMousePosition(Vector2 position) => Game.Window.MousePosition = position;



    public Vector2 GetMouseScroll() {
        return loopState switch {
            InputLoopState.Update => updateMouseState.Scroll,
            InputLoopState.Render => renderMouseState.Scroll,
            _ => Vector2.Zero
        };
    }



    /// <summary>
    /// returns the change in mouse scroll since the last frame
    /// if called during an update frame, it's relative to the previous update frame
    /// if called during a render frame, it's relative to the previous render frame
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMouseScrollDelta() {
        return loopState switch {
            InputLoopState.Update => updateMouseState.ScrollDelta,
            InputLoopState.Render => renderMouseState.ScrollDelta,
            _ => Vector2.Zero
        };
    }
}

internal enum InputLoopState {
    Update,
    Render,
}