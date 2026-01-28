
namespace FrogLib;

public class MinHeap {

    public int Count => next - ROOT;

    const int ROOT = 1;

    private int next = ROOT;

    private ExpandingArray<int> tree = new();



    public void Push(int number) {

        int index = next++;

        while (index > ROOT) {

            int parentIndex = index >> 1;
            int parent = tree[parentIndex];

            if (number >= parent) break;

            tree[index] = parent;
            index = parentIndex;
        }

        tree[index] = number;
    }

    public int Peek() {
        if (next == ROOT) throw new InvalidOperationException("Min-heap is empty");
        return tree[ROOT];
    }

    public int Pop() {

        if (next == ROOT) throw new InvalidOperationException("Min-heap is empty");
        if (next == ROOT + 1) return tree[--next];

        int index = ROOT;

        int result = tree[index];

        int last = tree[--next];

        while (true) {

            int leftIndex = index << 1;

            if (leftIndex >= next) break; // no children

            int rightIndex = leftIndex | 1;

            int smallerIndex = leftIndex;

            if (rightIndex < next && tree[rightIndex] < tree[leftIndex]) {
                smallerIndex = rightIndex;
            }

            if (tree[smallerIndex] >= last) break;

            tree[index] = tree[smallerIndex];
            index = smallerIndex;
        }

        tree[index] = last;
        return result;
    }

    public void Clear() => next = ROOT;
}