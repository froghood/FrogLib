
namespace FrogLib;

public unsafe class RunnerCallbacks {

    private delegate*<double, void> processEventsCallback;
    private delegate*<bool, void> updateCallback;
    private delegate*<bool, float, void> renderCallback;

    internal RunnerCallbacks(
        delegate*<double, void> processEventsCallback,
        delegate*<bool, void> updateCallback,
        delegate*<bool, float, void> renderCallback) {

        this.processEventsCallback = processEventsCallback;
        this.updateCallback = updateCallback;
        this.renderCallback = renderCallback;
    }

    public void ProcessEvents(double timeout = 0) => processEventsCallback(timeout);
    public void Update(bool isFixed) => updateCallback(isFixed);
    public void Render(bool isFixed, float alpha) => renderCallback(isFixed, alpha);
}