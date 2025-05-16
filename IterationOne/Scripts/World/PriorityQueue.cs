using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<KeyValuePair<TElement, TPriority>> BaseHeap = new ();

    public int Count => BaseHeap.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        Insert(element, priority);
    }

    public TElement Dequeue()
    {
        if (!IsEmpty)
        {
            var result = BaseHeap[0].Key;
            DeleteRoot();
            return result;
        }
        else
        {
            throw new InvalidOperationException("Priority queue is empty");
        }
    }

    public bool IsEmpty
    {
        get { return BaseHeap.Count == 0; }
    }

    public IEnumerable<KeyValuePair<TElement, TPriority>> UnorderedItems
    {
        get { return BaseHeap; }
    }

    private void Insert(TElement element, TPriority priority)
    {
        var val = new KeyValuePair<TElement, TPriority>(element, priority);
        BaseHeap.Add(val);

        HeapifyUp(BaseHeap.Count - 1);
    }

    private void DeleteRoot()
    {
        if (BaseHeap.Count <= 1)
        {
            BaseHeap.Clear();
            return;
        }

        BaseHeap[0] = BaseHeap[BaseHeap.Count - 1];
        BaseHeap.RemoveAt(BaseHeap.Count - 1);

        HeapifyDown(0);
    }

    private void HeapifyUp(int pos)
    {
        if (pos >= BaseHeap.Count) return;

        var parent = (pos - 1) / 2;
        if (parent >= 0 && BaseHeap[pos].Value.CompareTo(BaseHeap[parent].Value) < 0)
        {
            var temp = BaseHeap[pos];
            BaseHeap[pos] = BaseHeap[parent];
            BaseHeap[parent] = temp;

            HeapifyUp(parent);
        }
    }

    private void HeapifyDown(int pos)
    {
        if (pos >= BaseHeap.Count) return;

        var leftChild = 2 * pos + 1;
        var rightChild = 2 * pos + 2;
        var smallest = pos;

        if (leftChild < BaseHeap.Count && BaseHeap[smallest].Value.CompareTo(BaseHeap[leftChild].Value) > 0)
        {
            smallest = leftChild;
        }

        if (rightChild < BaseHeap.Count && BaseHeap[smallest].Value.CompareTo(BaseHeap[rightChild].Value) > 0)
        {
            smallest = rightChild;
        }

        if (smallest != pos)
        {
            var temp = BaseHeap[pos];
            BaseHeap[pos] = BaseHeap[smallest];
            BaseHeap[smallest] = temp;

            HeapifyDown(smallest);
        }
    }
}