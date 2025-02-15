using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FrogLib;

public static class Game {

    public static event Action<Vector2i>? WindowResized;

    public static bool IsRunning { get; private set; }
    public static Vector2 WindowSize { get => window.ClientSize; }
    public static CursorState CursorState { get => window.CursorState; set => window.CursorState = value; }
    public static bool IsFocused => window.IsFocused;
    public static IGLFWGraphicsContext Context { get => window.Context; }

    /// <summary>
    /// the time since the game has been started
    /// </summary>
    public static Time Time { get; private set; }

    /// <summary>
    /// the time between each run iteration
    /// <para> not intended for use, use UpdateDelta or RenderDelta instead </para>
    /// </summary>
    public static Time Delta { get; private set; }

    /// <summary>
    /// the time between the start of the current and previous update calls
    /// </summary>
    public static Time UpdateDelta { get; private set; }

    /// <summary>
    /// the time the last update call took to complete
    /// </summary>
    public static Time UpdateTime { get; private set; }

    /// <summary>
    /// the time between the start of the current and previous render calls
    /// </summary>
    public static Time RenderDelta { get; private set; }

    /// <summary>
    /// the time the last render call took to complete
    /// </summary>
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



    private static int targetUpdateFrequency;
    private static int targetRenderFrequency;
    private static Time previousRenderTime;
    private static Time previousUpdateTime;

    public static void Init(NativeWindowSettings settings) {

        systemProvider = new GameSystemProvider();

        // window
        window = new NativeWindow(settings);

        window.Closing += (_) => IsRunning = false;

        window.FramebufferResize += (e) => {
            WindowResized?.Invoke(new Vector2i(e.Width, e.Height));
        };

        // clock
        clock = new Stopwatch();

        // input
        Input = new Input(window);

        SetTargetUpdateFrequency(60);
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

        systemProvider.Startup();

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



    public static void Close() => IsRunning = false;



    public static T Get<T>() where T : GameSystem => systemProvider.Get<T>();
    public static T Register<T>() where T : GameSystem => systemProvider.Register<T>();



    private static void RunVariableCoupled() {

        Time previousTime = 0L;

        while (IsRunning) {

            Time time = GetTime();

            Time = time;
            Delta = time - previousTime;
            previousTime = time;

            Update();

            if (Delta.AsSeconds() * targetUpdateFrequency <= 1f) Render(1f); // skip 
        }
    }

    private static void RunFixedDecoupled() {

        long totalUpdateCount = 0L;
        long totalRenderCount = 0L;

        while (IsRunning) {

            Time time = GetTime();

            Time = time;
            Delta = (long)Math.Round(1000000d / targetUpdateFrequency);

            if (time * targetUpdateFrequency >= totalUpdateCount * 1000000L) {

                Update();
                totalUpdateCount++;

            } else {

                if (time * targetRenderFrequency >= totalRenderCount * 1000000L) {

                    Time nearestRenderCount = time * targetRenderFrequency / 1000000L;

                    float alpha = nearestRenderCount * targetUpdateFrequency % targetRenderFrequency / (float)targetRenderFrequency;
                    Render(alpha);

                    totalRenderCount = nearestRenderCount + 1;

                }
            }
        }
    }

    /// <summary>
    /// sets the target update frequency
    /// </summary>
    /// <param name="frequency"> the target update frequency in hertz</param>
    public static void SetTargetUpdateFrequency(int frequency) {
        targetUpdateFrequency = Math.Max(frequency, 1);
    }

    /// <summary>
    /// sets the target render frequency
    /// </summary>
    /// <param name="frequency"> the target render frequency in hertz</param>
    public static void SetTargetRenderFrequency(int frequency) {
        targetRenderFrequency = Math.Max(frequency, 1);
    }

    private static void Update() {

        Time time = GetTime();

        Input.PollInputs();
        systemProvider.Update();

        UpdateTime = GetTime() - time;
        UpdateDelta = time - previousUpdateTime;
        previousUpdateTime = time;
    }

    private static void Render(float alpha) {

        Time time = GetTime();

        systemProvider.Render(alpha);

        RenderTime = GetTime() - time;
        RenderDelta = time - previousRenderTime;
        previousRenderTime = time;

        Frame++;
    }

    private static Time GetTime() => clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);

}
