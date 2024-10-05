using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FrogLib;

public static class Game {

    public static bool IsRunning { get; private set; }
    public static Vector2 WindowSize { get => window.ClientSize; }
    public static CursorState CursorState { get => window.CursorState; set => window.CursorState = value; }
    public static bool IsFocused => window.IsFocused;
    public static IGLFWGraphicsContext Context { get => window.Context; }
    public static Time Time { get; private set; }
    public static Time Delta { get; private set; }

    public static Time UpdateTime { get; private set; }
    public static Time RenderTime { get; private set; }

    /// <summary>
    /// the total number of frames rendered so far
    /// </summary>
    public static long Frame { get; private set; }



    [AllowNull]
    public static Input Input { get; private set; }


    [AllowNull]
    private static GameSystemProvider systemProvider;

    [AllowNull]
    private static NativeWindow window;

    [AllowNull]
    private static Stopwatch clock;



    private static int updateFrequency;
    private static int renderFrequency;
    private static Time previousRenderTime;
    private static Time previousUpdateTime;

    public static void Init(NativeWindowSettings settings) {

        systemProvider = new GameSystemProvider();

        // window
        window = new NativeWindow(settings);

        window.Closing += (_) => IsRunning = false;

        window.FramebufferResize += (e) => {
            Log.Info(e.Size);
            GL.Viewport(0, 0, e.Width, e.Height);
        };

        // clock
        clock = new Stopwatch();

        // input
        Input = new Input(window);

        SetUpdateFrequency(60);
    }



    public static void Init(Vector2i size) {

        var settings = new NativeWindowSettings() {
            ClientSize = size,
            StartVisible = false,

        };

        Game.Init(settings);

    }



    public static void Run(RunType type) {

        if (IsRunning) return;
        IsRunning = true;

        window.IsVisible = true;

        clock.Restart();

        var methods = new Dictionary<RunType, Action>() {
            {RunType.VariableCoupled, RunVariableCoupled},
            {RunType.FixedDecoupled, RunFixedDecoupled}
        };

        methods[type].Invoke();

        systemProvider.Shutdown();

        window.Dispose();
    }



    public static T Get<T>() where T : GameSystem => systemProvider.Get<T>();
    public static T Register<T>() where T : GameSystem => systemProvider.Register<T>();



    private static void RunVariableCoupled() {

        Time previousTime = 0L;

        while (IsRunning) {

            Time time = clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);

            Time = time;
            Delta = time - previousTime;
            previousTime = time;

            Update();

            if (Delta.AsSeconds() * updateFrequency <= 1f) Render(1f); // skip 
        }
    }

    private static void RunFixedDecoupled() {

        long totalUpdateCount = 0L;
        long totalRenderCount = 0L;

        while (IsRunning) {

            Time time = clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);

            Time = time;
            Delta = (long)Math.Round(1000000d / updateFrequency);

            if (time * updateFrequency >= totalUpdateCount * 1000000L) {

                Update();
                totalUpdateCount++;

            } else {

                if (time * renderFrequency >= totalRenderCount * 1000000L) {

                    Time nearestRenderCount = time * renderFrequency / 1000000L;

                    float alpha = nearestRenderCount * updateFrequency % renderFrequency / (float)renderFrequency;
                    Render(alpha);

                    totalRenderCount = nearestRenderCount + 1;

                }
            }
        }
    }

    public static void SetUpdateFrequency(int frequency) {
        updateFrequency = Math.Max(frequency, 1);
    }

    public static void SetRenderFrequency(int frequency) {
        renderFrequency = Math.Max(frequency, 1);
    }

    private static void Update() {

        Time time = clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);
        UpdateTime = time - previousUpdateTime;
        previousUpdateTime = time;

        Input.PollInputs();

        systemProvider.Update();
    }

    private static void Render(float alpha) {

        Time time = clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);
        RenderTime = time - previousRenderTime;
        previousRenderTime = time;

        systemProvider.Render(alpha);
        Frame++;
    }

}
