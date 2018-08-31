using System;
using System.Collections.Generic;

namespace Crystal.Collections
{
    public class PriorityQueueSimple<T> : IPriorityQueueSimple<T>
    {
        private static readonly int DEFAULT_CAPACITY = 16;

        IComparer<T> comparer;

        T[] heap;

        public int Count
        {
            private set;
            get;
        }

        public PriorityQueueSimple() : this(DEFAULT_CAPACITY, null)
        {
        }

        public PriorityQueueSimple(int capacity) : this(capacity, null)
        {
        }

        public PriorityQueueSimple(IComparer<T> comparer) : this(DEFAULT_CAPACITY, comparer)
        {
        }

        public PriorityQueueSimple(int capacity, IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
            this.heap = new T[capacity <= 0 ? DEFAULT_CAPACITY : capacity];
        }

        public void Push(T t)
        {
            if (Count >= heap.Length)
            {
                Array.Resize(ref heap, Count << 1);
            }
            heap[Count] = t;
            ShiftUp(Count++);
        }

        public T Pop()
        {
            var peek = Peek();
            heap[0] = heap[--Count];
            if (Count > 0)
            {
                ShiftDown(0);
            }
            return peek;
        }

        public T Peek()
        {
            if (Count > 0)
            {
                return heap[0];
            }
            throw new InvalidOperationException("Priority Queue is null.");
        }

        private void ShiftUp(int n)
        {
            var v = heap[n];
            for (var n2 = n >> 1; n > 0 && comparer.Compare(v, heap[n2]) > 0; n = n2, n2 >>= 1)
                heap[n] = heap[n2];
            heap[n] = v;
        }

        private void ShiftDown(int n)
        {
            var v = heap[n];
            for (var n2 = n << 1; n2 < Count; n = n2, n2 <<= 1)
            {
                if (n2 + 1 < Count && comparer.Compare(heap[n2 + 1], heap[n2]) > 0)
                    n2++;
                if (comparer.Compare(v, heap[n2]) >= 0)
                    break;
                heap[n] = heap[n2];
            }
            heap[n] = v;
        }
    }

    public interface IPriorityQueueSimple<T>
    {

        void Push(T t);

        T Pop();

        T Peek();
    }
}


