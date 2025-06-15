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
    public static Time Time => currentRunner?.Time ?? 0UL;

    /// <summary>
    /// the time between each run iteration
    /// <para> not intended for use, use UpdateDelta or RenderDelta instead </para>
    /// </summary>
    public static Time Delta => currentRunner?.Delta ?? 0UL;



    /// <summary>
    /// the time between the start of the current and previous update calls
    /// </summary>
    public static Time UpdateDelta => currentRunner?.UpdateDelta ?? 0UL;

    /// <summary>
    /// the time between the start of the current and previous render calls
    /// </summary>
    public static Time RenderDelta => currentRunner?.RenderDelta ?? 0UL;



    /// <summary>
    /// the total number of update calls so far
    /// </summary>
    public static uint UpdateCount => currentRunner?.UpdateCount ?? 0U;

    /// <summary>
    /// the total number of render calls so far
    /// </summary>
    public static uint RenderCount => currentRunner?.RenderCount ?? 0U;



    /// <summary>
    /// the time the last update call took to complete
    /// </summary>
    public static Time UpdateTime => currentRunner?.UpdateTime ?? 0UL;

    /// <summary>
    /// the time the last render call took to complete
    /// </summary>
    public static Time RenderTime => currentRunner?.RenderTime ?? 0UL;



    private static bool isInitialized;

    private static NativeWindow? window;

    private readonly static GameSystemProvider systemProvider = new();

    private readonly static Dictionary<Type, object> resources = new();



    // private readonly static Stopwatch clock;
    // private static int targetUpdateFrequency;
    // private static int targetRenderFrequency;
    // private static Time previousUpdateTime;
    // private static Time previousRenderTime;



    private static IRunner? currentRunner;



    static Game() {
        systemProvider = new GameSystemProvider();
        // clock = new Stopwatch();
        // clock.Reset();
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



    public unsafe static void Run(IRunner runner) {

        ThrowIfNotInitialized();
        ThrowIfRunning();

        IsRunning = true;

        window!.IsVisible = true;

        //clock!.Restart();
        systemProvider!.Startup();

        currentRunner = runner;
        var runnerCallbacks = new RunnerCallbacks(&ProcessEvents, &Update, &Render);

        while (IsRunning) {
            runner.Run(runnerCallbacks);
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



    private static void ProcessEvents(double timeout = 0) => window!.ProcessEvents(timeout);

    private static void Update() => systemProvider?.Update();

    private static void Render(float alpha) => systemProvider?.Render(alpha);



    // private static Time GetTime() => (ulong)clock.ElapsedTicks / ((ulong)Stopwatch.Frequency / 1_000_000UL);



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