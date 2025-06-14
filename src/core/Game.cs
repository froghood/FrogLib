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
            ThrowIfNotInitialized();
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
    private static Time previousUpdateTime;
    private static Time previousRenderTime;



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



    public unsafe static void Run<T>() where T : IRunner, new() {

        ThrowIfNotInitialized();
        ThrowIfRunning();

        IsRunning = true;

        window!.IsVisible = true;

        clock!.Restart();
        systemProvider!.Startup();

        var runner = new T();
        var runnerCallbacks = new RunnerCallbacks(&ProcessEvents, &Update, &Render);

        Time previousTime = 0L;

        while (IsRunning) {

            var time = GetTime();
            var delta = time - previousTime;
            previousTime = time;

            Time = time;
            Delta = delta;

            runner.Run(runnerCallbacks, time, delta, targetUpdateFrequency, targetRenderFrequency);
        }

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



    private static void ProcessEvents(double timeout = 0) => window!.ProcessEvents(timeout);

    private static void Update(bool isFixed) {

        Time time = GetTime();

        systemProvider?.Update();

        UpdateTime = GetTime() - time;
        UpdateDelta = isFixed ? 1_000_000L / targetUpdateFrequency : time - previousUpdateTime;
        previousUpdateTime = time;

        UpdateCount++;
    }

    private static void Render(bool isFixed, float alpha) {

        Time time = GetTime();

        systemProvider?.Render(alpha);

        RenderTime = GetTime() - time;
        RenderDelta = isFixed ? 1_000_000L / targetRenderFrequency : time - previousRenderTime;
        previousRenderTime = time;

        RenderCount++;
    }



    private static Time GetTime() => clock.ElapsedTicks / (Stopwatch.Frequency / 1000000L);



    private static void ThrowIfNotInitialized() {
        if (!isInitialized) {
            throw new Exception("Game has not been initialized");
        }
    }

    private static void ThrowIfRunning() {
        if (IsRunning) {
            throw new Exception("Game is not running");
        }
    }
}