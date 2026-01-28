
using System.Collections;
using System.Runtime.CompilerServices;

namespace FrogLib;


/// <summary>
/// warning: refs will break when array is resized
/// </summary>
/// <typeparam name="T"></typeparam>
public class FastStack<T> : IEnumerable<T> {

    public int Capacity => items.Length;
    public int Length => count;



    private int count;

    private ExpandingArray<T> items;

    private bool isRefType;



    public FastStack() {
        items = new ExpandingArray<T>();

        isRefType = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
    }



    public ref T this[int index] {
        get {
            ThrowIfIndexOutOfRange(index);
            return ref items[index];
        }
    }



    public int Push(in T value) {
        int index = count;
        items[index] = value;
        count++;
        return index;
    }

    public int Push(in ReadOnlySpan<T> values) {
        int index = count;

        for (int i = 0; i < values.Length; i++) {
            items[index + i] = values[i];
        }

        return index;
    }



    public void Insert(in T value, int index) {

        if (index < 0 || index > count) throw new IndexOutOfRangeException();

        for (int i = count - 1; i >= index; i--) items[i + 1] = items[i];

        items[index] = value;
        count++;
    }



    public T Pop() {
        if (count == 0) throw new InvalidOperationException("Stack is empty");

        count--;

        var item = items[count];

        if (isRefType) items[count] = default!;

        return item;
    }

    public bool TryPop(out T item) {
        if (count == 0) {
            item = default!;
            return false;
        }

        item = Pop();
        return true;
    }



    public ref T Peek() {
        if (count == 0) throw new InvalidOperationException("Stack is empty");

        return ref items[count - 1];
    }

    public bool TryPeek(out T item) {
        if (count == 0) {
            item = default!;
            return false;
        } else {
            item = items[count - 1];
            return true;
        }
    }



    public T Remove(int index) {

        ThrowIfIndexOutOfRange(index);

        var item = items[index];

        for (int i = index; i < count - 1; i++) items[i] = items[i + 1];

        count--;

        return item;
    }



    public ReadOnlySpan<T> Span(int index, int count) {

        if (index < 0 || index + count > this.count) throw new IndexOutOfRangeException();

        return new ReadOnlySpan<T>(items, index, count);
    }

    public ReadOnlySpan<T> Span() => count > 0 ? new ReadOnlySpan<T>(items, 0, count) : ReadOnlySpan<T>.Empty;



    public void Sort(Comparison<T> comparison) {
        if (count == 0) return;
        var span = new Span<T>(items, 0, count);
        span.Sort(comparison);
    }



    public void Clear() {
        if (isRefType) Array.Clear(items);
        count = 0;
    }



    public IEnumerator<T> GetEnumerator() {
        for (int i = 0; i < count; i++) yield return items[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



    private void ThrowIfIndexOutOfRange(int index) {
        if (index < 0 || index >= count) throw new IndexOutOfRangeException($"Index {index} is out of range");
    }
}