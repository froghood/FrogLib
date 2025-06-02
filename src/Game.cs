using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FrogLib.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace FrogLib;

public static class Game {



    public static NativeWindow Window {
        get {
            if (!isInitialized) {
                throw new InvalidOperationException("Game has not been initialized");
            }
            return window!;
        }
    }



    public static bool IsRunning { get; private set; }

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
    /// the total number of update calls so far
    /// </summary>
    public static long UpdateCount { get; private set; }

    /// <summary>
    /// the time between the start of the current and previous render calls
    /// </summary>
    public static Time RenderDelta { get; private set; }

    /// <summary>
    /// the time the last render call took to complete
    /// </summary>
    public static Time RenderTime { get; private set; }

    /// <summary>
    /// the total number of render calls so far
    /// </summary>
    public static long RenderCount { get; private set; }



    private static bool isInitialized;
    private static NativeWindow? window;
    private readonly static GameSystemProvider systemProvider = new();
    private readonly static Dictionary<Type, object> resources = new();
    private readonly static Stopwatch clock;



    private static int targetUpdateFrequency;
    private static int targetRenderFrequency;
    private static Time previousRenderTime;
    private static Time previousUpdateTime;



    static Game() {
        systemProvider = new GameSystemProvider();
        clock = new Stopwatch();
        clock.Reset();
    }



    public static void Init(NativeWindowSettings settings) {

        window = new NativeWindow(settings);

        window.Closing += (_) => IsRunning = false;

        isInitialized = true;
    }

    public static void Init(Vec2i size) {

        var settings = new NativeWindowSettings() {
            ClientSize = size,
            StartVisible = false,

        };

        Init(settings);

    }



    public static void Run(RunType type) {

        if (!isInitialized) {
            throw new Exception("Game has not been initialized");
        }

        if (IsRunning) {
            throw new Exception("Game is already running");
        }

        if (IsRunning) return;
        IsRunning = true;

        window!.IsVisible = true;
        clock!.Restart();
        systemProvider!.Startup();


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




    public static T AddResource<T>([DisallowNull] T resource) {
        var type = typeof(T);
        if (resources.ContainsKey(type)) {
            Log.Warn($"Resource \"{typeof(T).Name}\" already present in the library.");
        }
        resources[type] = resource;
        return resource;
    }

    public static T GetResource<T>() {
        var type = typeof(T);
        if (!resources.TryGetValue(type, out object? resource)) {
            throw new Exception($"Resource \"{type.Name}\" not found");
        }
        return (T)resource!;
    }




    private static void RunVariableCoupled() {

        Time previousTime = 0L;

        while (IsRunning) {

            Window.ProcessEvents(0);

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

            Window.ProcessEvents(0);

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

        systemProvider?.Update();

        UpdateTime = GetTime() - time;
        UpdateDelta = time - previousUpdateTime;
        previousUpdateTime = time;

        UpdateCount++;
    }

    private static void Render(float alpha) {

        Time time = GetTime();

        systemProvider?.Render(alpha);

        RenderTime = GetTime() - time;
        RenderDelta = time - previousRenderTime;
        previousRenderTime = time;

        RenderCount++;
    }

    private static Time GetTime() => clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);

}
