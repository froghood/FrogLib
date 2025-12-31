
using System.Runtime.CompilerServices;

namespace FrogLib;

public class HeapStorage<T> {

    public int Count => defaultIndex - priorityIndices.Count;

    public int NextIndex => priorityIndices.Count > 0 ? priorityIndices.Peek() : defaultIndex;



    private int defaultIndex;

    private ExpandingArray<T> items = new();
    private ExpandingArray<bool> reserved = new();
    private MinHeap priorityIndices = new();

    private bool isRefType;



    public HeapStorage() {
        isRefType = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
    }



    public ref T this[int index] {
        get {
            ThrowIfUnreserved(index);
            return ref items[index];
        }
    }



    public int Alloc() {
        int index = priorityIndices.Count > 0 ? priorityIndices.Pop() : defaultIndex++;

        reserved[index] = true;

        return index;
    }

    public int Alloc(in T defaultValue) {

        int index = Alloc();

        items[index] = defaultValue;

        return index;
    }



    public void Free(int index) {

        ThrowIfUnreserved(index);

        priorityIndices.Push(index);

        if (isRefType) items[index] = default!;
        reserved[index] = false;

    }



    public bool IsReserved(int index) => index >= 0 && index < items.Length && reserved[index];



    public void Clear() {

        defaultIndex = 0;

        priorityIndices.Clear();

        if (isRefType) Array.Clear(items);
        Array.Clear(reserved);
    }



    private void ThrowIfUnreserved(int index) {
        if (!IsReserved(index)) throw new AccessViolationException($"Attempted to read or write to unreserved storage index {index}");
    }
}