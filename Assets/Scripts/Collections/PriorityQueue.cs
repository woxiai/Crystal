using System;
using System.Collections;
using System.Collections.Generic;

namespace Crystal.Collections
{
    /// <summary>
    /// 优先队列 （参考Java PriorityQueue）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        private static readonly int DEFAULT_CAPACITY = 16;

        private static readonly int MAX_CAPACITY = int.MaxValue - 8;

        private Comparer<T> comparer;

        private T[] queue;

        private int _size;

        public int Count { get { return _size; } }

        public PriorityQueue() : this(DEFAULT_CAPACITY, null)
        {
        }

        public PriorityQueue(int capacity) : this(capacity, null)
        {
        }

        public PriorityQueue(Comparer<T> comparer) : this(DEFAULT_CAPACITY, comparer)
        {
        }

        public PriorityQueue(int capacity, Comparer<T> comparer)
        {
            this.queue = new T[capacity <= 0 ? DEFAULT_CAPACITY : capacity];
            this.comparer = comparer;
        }


        public void Clear()
        {
            for (int i = 0; i < _size; i++)
            {
                queue[i] = default(T);
            }
            _size = 0;
        }

        public bool Contains(T t)
        {
            if (t == null)
            {
                return false;
            }
            for (int i = 0; i < _size; i++)
            {
                if (t.Equals(queue[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Copy to array is null.");
            }
            if (arrayIndex < 0 || arrayIndex >= _size)
            {
                throw new ArgumentNullException("Copy to array, index must > 0 && < queue's Count");
            }
            Array.Copy(queue, arrayIndex, array, 0, array.Length);
        }

        public T Dequeue()
        {
            if (_size == 0)
            {
                return default(T);
            }
            var s = --_size;
            var result = queue[0];
            var x = queue[s];
            queue[s] = default(T);
            if (s != 0)
            {
                SiftDown(0, x);
            }
            return result;
        }

        public void Enqueue(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException();
            }
            var i = _size;
            if (i >= queue.Length)
            {
                Grow(i + 1);
            }
            SiftUp(i, t);
            _size = i + 1;
        }

        public T Peek()
        {
            return _size == 0 ? default(T) : queue[0];
        }

        public T[] ToArray()
        {
            if (_size == 0)
            {
                return null;
            }
            else
            {
                var arr = new T[_size];
                Array.Copy(queue, arr, _size);
                return arr;
            }
        }

        private void Grow(int minCapacity)
        {
            int oldCapacity = queue.Length;
            int newCapacity = oldCapacity + ((oldCapacity < 64) ? (oldCapacity + 2) : (oldCapacity >> 1));
            if (newCapacity - MAX_CAPACITY > 0)
            {
                newCapacity = HugeCapacity(minCapacity);
            }
            Array.Resize<T>(ref queue, newCapacity);
        }

        private static int HugeCapacity(int minCapacity)
        {
            if (minCapacity < 0)
            {
                throw new OutOfMemoryException("Priority Queue HugeCapacity Out Of Memory.");
            }
            return (minCapacity > MAX_CAPACITY) ? int.MaxValue : MAX_CAPACITY;
        }

        private void SiftDown(int k, T x)
        {
            if (comparer != null)
            {
                SiftDownComparer(k, x);
            }
            else
            {
                if (x is IComparable<T>)
                {
                    SiftDownComparable(k, x);
                }
                else
                {
                    comparer = Comparer<T>.Default;
                    SiftDownComparer(k, x);
                }
            }
        }

        private void SiftDownComparable(int k, T x)
        {
            var key = x as IComparable<T>;
            int half = _size >> 1;
            while (k < half)
            {
                int child = (k << 1) + 1;
                T t = queue[child];
                int right = child + 1;
                if (right < _size && (t as IComparable<T>).CompareTo(queue[right]) > 0)
                {
                    t = queue[child = right];
                }
                if (key.CompareTo(t) <= 0)
                {
                    break;
                }
                queue[k] = t;
                k = child;
            }
            queue[k] = x;
        }

        private void SiftDownComparer(int k, T x)
        {
            int half = _size >> 1;
            while (k < half)
            {
                int child = (k << 1) + 1;
                T t = queue[child];
                int right = child + 1;
                if (right < _size && comparer.Compare(t, queue[right]) > 0)
                {
                    t = queue[child = right];
                }
                if (comparer.Compare(x, t) <= 0)
                {
                    break;
                }
                queue[k] = t;
                k = child;
            }
            queue[k] = x;
        }


        private void SiftUp(int k, T x)
        {
            if (comparer != null)
            {
                SiftUpComparer(k, x);
            }
            else
            {
                if (x is IComparable)
                {
                    SiftUpComparable(k, x);
                }
                else
                {
                    comparer = Comparer<T>.Default;
                    SiftUpComparer(k, x);
                }
            }
        }

        private void SiftUpComparable(int k, T x)
        {
            var key = x as IComparable;
            while (k > 0)
            {
                var parent = (k - 1) >> 1;
                T t = queue[parent];
                if (key.CompareTo(t as IComparable) >= 0)
                {
                    break;
                }
                queue[k] = t;
                k = parent;
            }
            queue[k] = x;
        }

        private void SiftUpComparer(int k, T x)
        {
            while (k > 0)
            {
                int parent = (k - 1) >> 1;
                T t = queue[parent];
                if (comparer.Compare(x, t) >= 0)
                {
                    break;
                }
                queue[k] = t;
                k = parent;
            }
            queue[k] = x;
        }

        public void Foreach(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action is null.");
            }
            for (var i = 0; i < _size; i++)
            {
                action(queue[i]);
            }
        }

        public T Find(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                return default(T);
            }

            for (var i = 0; i < _size; i++)
            {
                if (predicate(queue[i]))
                {
                    return queue[i];
                }
            }
            return default(T);
        }

        public bool Contains(Predicate<T> predicate)
        {
            if (predicate == null)
            {
                return false;
            }
            for (var i = 0; i < _size; i++)
            {
                if (predicate(queue[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal interface IPriorityQueue<T>
    {
        void Enqueue(T t);

        T Dequeue();

        T Peek();

        bool Contains(T t);

        void Clear();

        T[] ToArray();

        void CopyTo(T[] array, int arrayIndex);

        void Foreach(Action<T> action);

        T Find(Predicate<T> predicate);

        bool Contains(Predicate<T> predicate);
    }
}