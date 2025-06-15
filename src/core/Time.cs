namespace FrogLib;

public struct Time {

    private ulong microseconds;

    public Time(ulong microseconds) => this.microseconds = microseconds;

    public static Time InSeconds(double seconds) => new Time((ulong)(seconds * 1000000d));
    public static Time InMilliseconds(double milliseconds) => new Time((ulong)(milliseconds * 1000d));

    public float AsSecondsF() => (float)AsSeconds();
    public double AsSeconds() => microseconds / 1000000d;
    public double AsMilliseconds() => microseconds / 1000d;

    public static Time operator +(Time left, Time right) => left.microseconds + right.microseconds;
    public static Time operator -(Time left, Time right) => left.microseconds - right.microseconds;
    public static Time operator *(Time left, Time right) => left.microseconds * right.microseconds;

    public static Time operator *(Time left, double scalar) => (ulong)(left.microseconds * scalar);

    public static implicit operator ulong(Time duration) => duration.microseconds;
    public static implicit operator Time(ulong amount) => new Time(amount);

    public override string ToString() => microseconds.ToString();

    public static Time Max(Time a, Time b) => (Time)Math.Max(a, b);
    public static Time Min(Time a, Time b) => (Time)Math.Min(a, b);
}