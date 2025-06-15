
using System.Diagnostics;

namespace FrogLib;

public class FixedRunner : IRunner {

    public Time Time { get; private set; }
    public Time Delta { get; private set; }

    public Time UpdateDelta { get; private set; }
    public Time RenderDelta { get; private set; }

    public Time UpdateTime { get; private set; }
    public Time RenderTime { get; private set; }

    public uint UpdateCount { get; private set; }
    public uint RenderCount { get; private set; }



    private readonly ulong targetUpdateFrequency;
    private readonly ulong maxRenderFrequency;



    private Stopwatch clock = new Stopwatch();

    private ulong previousTime;
    private ulong previousRenderTime;



    public FixedRunner(ulong targetUpdateFrequency, ulong maxRenderFrequency) {

        this.targetUpdateFrequency = targetUpdateFrequency;
        this.maxRenderFrequency = maxRenderFrequency;

        UpdateDelta = new Time(1_000_000UL / targetUpdateFrequency);

        clock.Start();
    }



    public void Run(RunnerCallbacks callbacks) {

        callbacks.ProcessEvents();

        ulong time = GetTime();
        ulong delta = time - previousTime;
        previousTime = time;

        ulong updateThreshold = time * targetUpdateFrequency;
        ulong renderThreshold = time * maxRenderFrequency;

        Time = time;
        Delta = delta;

        while (UpdateCount * 1_000_000UL <= updateThreshold) {
            Update(callbacks);
        }

        if (RenderCount * 1_000_000UL <= renderThreshold) {

            ulong lastUpdateCountTime = (RenderCount - 1) * 1_000_000UL;
            float alpha = (updateThreshold - lastUpdateCountTime) / 1_000_000f;

            Render(callbacks, alpha);
        }
    }



    private void Update(RunnerCallbacks callbacks) {

        ulong time = GetTime();

        callbacks.Update();

        UpdateTime = GetTime() - time;

        UpdateCount++;
    }

    private void Render(RunnerCallbacks callbacks, float alpha) {

        ulong time = GetTime();

        callbacks.Render(alpha);

        RenderTime = GetTime() - time;

        RenderDelta = time - previousRenderTime;
        previousRenderTime = time;

        RenderCount++;
    }



    private ulong GetTime() => (ulong)clock.ElapsedTicks / ((ulong)Stopwatch.Frequency / 1_000_000UL);
}



public class VariableRunner : IRunner {

    public Time Time { get; private set; }
    public Time Delta { get; private set; }

    public Time UpdateDelta { get; private set; }
    public Time RenderDelta { get; private set; }

    public Time UpdateTime { get; private set; }
    public Time RenderTime { get; private set; }

    public uint UpdateCount { get; private set; }
    public uint RenderCount { get; private set; }



    private readonly ulong maxRenderFrequency;



    private readonly Stopwatch clock = new Stopwatch();

    private ulong previousTime;
    private ulong previousUpdateTime;
    private ulong previousRenderTime;



    public VariableRunner(ulong maxRenderFrequency) {
        this.maxRenderFrequency = maxRenderFrequency;

        clock.Start();
    }



    public void Run(RunnerCallbacks callbacks) {

        callbacks.ProcessEvents();

        var time = GetTime();
        var delta = time - previousTime;
        previousTime = time;

        Time = time;
        Delta = delta;

        Update(callbacks);

        if (RenderCount * 1_000_000UL <= time * maxRenderFrequency) {
            Render(callbacks, 1f);
        }
    }



    private void Update(RunnerCallbacks callbacks) {

        ulong time = GetTime();

        callbacks.Update();

        UpdateTime = GetTime() - time;

        UpdateDelta = time - previousUpdateTime;
        previousUpdateTime = time;

        UpdateCount++;
    }

    private void Render(RunnerCallbacks callbacks, float alpha) {

        ulong time = GetTime();

        callbacks.Render(alpha);

        RenderTime = GetTime() - time;

        RenderDelta = time - previousRenderTime;
        previousRenderTime = time;

        RenderCount++;
    }



    private ulong GetTime() => (ulong)clock.ElapsedTicks / ((ulong)Stopwatch.Frequency / 1_000_000UL);
}