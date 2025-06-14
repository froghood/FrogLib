
namespace FrogLib;

public class FixedRunner : IRunner {

    private long totalUpdateCount = 0L;
    private long totalRenderCount = 0L;

    public void Run(RunnerCallbacks callbacks, Time time, Time delta, int targetUpdateFrequency, int targetRenderFrequency) {

        callbacks.ProcessEvents();

        if (time * targetUpdateFrequency >= totalUpdateCount * 1000000L) {

            callbacks.Update(true);
            totalUpdateCount++;

        } else {

            if (time * targetRenderFrequency >= totalRenderCount * 1000000L) {

                Time nearestRenderCount = time * targetRenderFrequency / 1000000L;

                float alpha = nearestRenderCount * targetUpdateFrequency % targetRenderFrequency / (float)targetRenderFrequency;
                callbacks.Render(false, alpha);

                totalRenderCount = nearestRenderCount + 1;

            }
        }
    }
}



public class VariableRunner : IRunner {

    public void Run(RunnerCallbacks callbacks, Time time, Time delta, int targetUpdateFrequency, int targetRenderFrequency) {

        callbacks.ProcessEvents();

        callbacks.Update(false);
        if (delta.AsSeconds() * targetRenderFrequency <= 1f) callbacks.Render(false, 1f);
    }
}