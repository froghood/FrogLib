
using System.Collections;

namespace FrogLib;

public struct ExpandingArray<T> : IEnumerable<T> {

    public int Length => items.Length;

    private T[] items;

    public ExpandingArray() {
        items = new T[1];
    }

    public static implicit operator T[](ExpandingArray<T> array) => array.items;

    public ref T this[int index] {
        get {
            while (index >= items.Length) Resize();
            return ref items[index];
        }
    }

    private void Resize() {
        int capacity = items.Length * 2;
        Array.Resize(ref items, capacity);
    }

    public IEnumerator<T> GetEnumerator() {
        for (int i = 0; i < items.Length; i++) yield return items[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}