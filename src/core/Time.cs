namespace FrogLib;

public struct Time {

    private long microseconds;

    public Time(long microseconds) => this.microseconds = microseconds;

    public static Time InSeconds(float seconds) => new Time((long)Math.Round(seconds * 1000000f));
    public static Time InMilliseconds(double milliseconds) => new Time((long)Math.Round(milliseconds * 1000d));

    public float AsSeconds() => microseconds / 1000000f;
    public double AsMilliseconds() => microseconds / 1000d;

    public static Time operator +(Time left, Time right) => left.microseconds + right.microseconds;
    public static Time operator -(Time left, Time right) => left.microseconds - right.microseconds;
    public static Time operator *(Time left, Time right) => left.microseconds * right.microseconds;

    public static Time operator *(Time left, double scalar) => (long)(left.microseconds * scalar);

    public static implicit operator long(Time duration) => duration.microseconds;
    public static implicit operator Time(long amount) => new Time(amount);

    public override string ToString() => microseconds.ToString();

    public static Time Max(Time a, Time b) => (Time)Math.Max(a, b);
    public static Time Min(Time a, Time b) => (Time)Math.Min(a, b);
}