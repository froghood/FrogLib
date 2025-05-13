using System.Collections;
using OpenTK.Windowing.GraphicsLibraryFramework;

internal unsafe class KeyboardState {

    private BitArray keys = new BitArray((int)Keys.LastKey + 1);
    private BitArray previousKeys = new BitArray((int)Keys.LastKey + 1);


    internal void Set(KeyboardState state) {
        for (int i = 0; i < keys.Length; i++) {
            previousKeys[i] = keys[i];
            keys[i] = state.IsKeyDown((Keys)i);
        }
    }

    internal void Set(OpenTK.Windowing.GraphicsLibraryFramework.KeyboardState state) {
        for (int i = 0; i < keys.Length; i++) {
            previousKeys[i] = keys[i];
            keys[i] = state.IsKeyDown((Keys)i);
        }
    }

    internal bool IsKeyDown(Keys key) => keys[(int)key];
    internal bool IsKeyPressed(Keys key) => keys[(int)key] && !previousKeys[(int)key];
    internal bool IsKeyReleased(Keys key) => !keys[(int)key] && previousKeys[(int)key];
}