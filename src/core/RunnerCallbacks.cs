
namespace FrogLib;

public unsafe class RunnerCallbacks {

    private delegate*<double, void> processEventsCallback;
    private delegate*<void> updateCallback;
    private delegate*<float, void> renderCallback;

    internal RunnerCallbacks(
        delegate*<double, void> processEventsCallback,
        delegate*<void> updateCallback,
        delegate*<float, void> renderCallback) {

        this.processEventsCallback = processEventsCallback;
        this.updateCallback = updateCallback;
        this.renderCallback = renderCallback;
    }

    public void ProcessEvents(double timeout = 0) => processEventsCallback(timeout);
    public void Update() => updateCallback();
    public void Render(float alpha) => renderCallback(alpha);
}