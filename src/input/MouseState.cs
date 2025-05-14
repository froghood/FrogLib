
using System.Collections;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

internal class MouseState {

    private const int NUM_BUTTONS = 16; // copied from OpenTK.Windowing.GraphicsLibraryFramework.MouseState.MaxButtons


    internal Vector2 Position { get; private set; }
    internal Vector2 PreviousPosition { get; private set; }
    internal Vector2 PositionDelta => Position - PreviousPosition;

    internal Vector2 Scroll { get; private set; }
    internal Vector2 PreviousScroll { get; private set; }
    internal Vector2 ScrollDelta => Scroll - PreviousScroll;

    private BitArray buttons = new BitArray(NUM_BUTTONS);
    private BitArray previousButtons = new BitArray(NUM_BUTTONS);

    internal void Set(MouseState state) {
        PreviousPosition = Position;
        Position = state.Position;
        PreviousScroll = Scroll;
        Scroll = state.Scroll;
        for (int i = 0; i < buttons.Length; i++) {
            previousButtons[i] = state.buttons[i];
            buttons[i] = state.IsButtonDown((MouseButton)i);
        }
    }

    internal void Set(OpenTK.Windowing.GraphicsLibraryFramework.MouseState state) {
        PreviousPosition = Position;
        Position = state.Position;
        PreviousScroll = Scroll;
        Scroll = state.Scroll;
        for (int i = 0; i < buttons.Length; i++) {
            previousButtons[i] = buttons[i];
            buttons[i] = state.IsButtonDown((MouseButton)i);
        }
    }

    internal bool IsButtonDown(MouseButton button) => buttons[(int)button];
    internal bool IsButtonPressed(MouseButton button) => buttons[(int)button] && !previousButtons[(int)button];
    internal bool IsButtonReleased(MouseButton button) => !buttons[(int)button] && previousButtons[(int)button];
}