
using System.Collections;
using FrogLib.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

internal class MouseState {

    private const int NUM_BUTTONS = 16; // copied from OpenTK.Windowing.GraphicsLibraryFramework.MouseState.MaxButtons


    internal Vec2 Position { get; private set; }
    internal Vec2 PreviousPosition { get; private set; }
    internal Vec2 PositionDelta => Position - PreviousPosition;

    internal Vec2 Scroll { get; private set; }
    internal Vec2 PreviousScroll { get; private set; }
    internal Vec2 ScrollDelta => Scroll - PreviousScroll;

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