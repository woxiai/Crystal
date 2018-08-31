using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool<T> : IObjectPool<T> where T : new()
{

    private static readonly int DEFAULT_CAPACITY = 16;

    private Stack<T> m_Stack;

    public int Capacity
    {
        private set;
        get;
    }

    public int Count
    {
        get
        {
            return m_Stack.Count;
        }
    }

    private UnityAction<T> onGetAction;

    private UnityAction<T> onRecycleAction;

    public ObjectPool(int capacity, UnityAction<T> onGetAction = null, UnityAction<T> onRecycleAction = null)
    {
        this.onGetAction = onGetAction;
        this.onRecycleAction = onRecycleAction;
        m_Stack = new Stack<T>(capacity <= DEFAULT_CAPACITY ? Mathf.ClosestPowerOfTwo(capacity) : DEFAULT_CAPACITY);
        Capacity = capacity;
    }

    public T Get()
    {
        T t = m_Stack.Count > 0 ? m_Stack.Pop() : Activator.CreateInstance<T>();
        onGetAction?.Invoke(t);
        return t;
    }

    public void Recycle(T t)
    {
        if (m_Stack.Count > 0 && m_Stack.Count < Capacity && !ReferenceEquals(m_Stack.Peek(), t))
        {
            m_Stack.Push(t);
        }
        onRecycleAction?.Invoke(t);
    }

    public void Dispose()
    {
        this.onGetAction = null;
        this.onRecycleAction = null;
        if (m_Stack != null)
        {
            m_Stack.Clear();
        }
    }

    public void Clear()
    {
        m_Stack.Clear();
    }
}

internal interface IObjectPool<T> : IDisposable where T : new()
{
    int Capacity
    {
        get;
    }

    int Count
    {
        get;
    }

    T Get();

    void Recycle(T t);

    void Clear();
}
