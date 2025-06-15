
namespace FrogLib;

public interface IRunner {

    Time Time { get; }
    Time Delta { get; }

    Time UpdateDelta { get; }
    Time RenderDelta { get; }

    Time UpdateTime { get; }
    Time RenderTime { get; }

    uint UpdateCount { get; }
    uint RenderCount { get; }

    void Run(RunnerCallbacks callbacks);
}