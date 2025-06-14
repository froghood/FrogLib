
namespace FrogLib;

public interface IRunner {
    void Run(RunnerCallbacks callbacks, Time time, Time delta, int targetUpdateFrequency, int targetRenderFrequency);
}