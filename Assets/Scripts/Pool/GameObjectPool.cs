using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crystal.Pool
{
    /// <summary>
    /// GameObject 缓存池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameObjectPool<T> : IGameObjectPool<T> where T : Component
    {

        private static readonly int DEFAULT_CAPACITY = 16;

        public Transform PoolTransform
        {
            private set;
            get;
        }

        public int Capacity
        {
            private set;
            get;
        }

        public int Count => m_Stack.Count;

        private Stack<T> m_Stack;

        private IGameObjectPoolFactory<T> poolFactory = null;

        public GameObjectPool(Transform poolParentTrans, IGameObjectPoolFactory<T> poolFactory, int capacity)
        {
            this.m_Stack = new Stack<T>(capacity <= DEFAULT_CAPACITY ? Mathf.ClosestPowerOfTwo(capacity) : DEFAULT_CAPACITY);
            this.Capacity = capacity;
            this.PoolTransform = poolParentTrans;
            this.poolFactory = poolFactory;
        }

        public T Get()
        {
            if (m_Stack.Count > 0)
            {
                var t = m_Stack.Pop();
                poolFactory?.OnGet(t);
                return t;
            }
            return poolFactory?.Instaniate<T>(null);
        }

        public T Get(Transform parent)
        {
            if (m_Stack.Count > 0)
            {
                var t = m_Stack.Pop();
                if (poolFactory == null)
                {
                    t.transform.SetParent(parent);
                }
                else
                {
                    poolFactory.OnGet(t, parent);
                }
                return t;
            }
            return poolFactory?.Instaniate<T>(parent);
        }

        public T Get(Transform parent, Vector3 position, Quaternion quaternion)
        {
            if (m_Stack.Count > 0)
            {
                var t = m_Stack.Pop();
                if (poolFactory == null)
                {
                    var trans = t.transform;
                    trans.SetParent(parent);
                    trans.localPosition = position;
                    trans.localRotation = quaternion;
                }
                else
                {
                    poolFactory.OnGet(t, parent, position, quaternion);
                }
                return t;
            }
            return poolFactory?.Instaniate<T>(parent);
        }

        public T Get(Transform parent, Vector3 position, Quaternion quaternion, Vector3 scale)
        {
            if (m_Stack.Count > 0)
            {
                var t = m_Stack.Pop();
                if (poolFactory == null)
                {
                    var trans = t.transform;
                    trans.SetParent(parent);
                    trans.localPosition = position;
                    trans.localRotation = quaternion;
                    trans.localScale = scale;
                }
                else
                {
                    poolFactory?.OnGet(t, parent, position, quaternion, scale);
                }
                return t;
            }
            return poolFactory?.Instaniate<T>(parent, position, quaternion, scale);
        }

        public void Recycle(T t)
        {
            if (t == null)
            {
                return;
            }
            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), t))
            {
                return;
            }
            if (m_Stack.Count < Capacity)
            {
                m_Stack.Push(t);
                t.transform.SetParent(PoolTransform);
                poolFactory?.OnRecycle(t);
            }
            else
            {
                if (poolFactory == null)
                {
                    GameObject.Destroy(t);
                }
                else
                {
                    poolFactory.OnDestroy(t);
                }
            }
        }

        public void Clear()
        {
            if (poolFactory == null)
            {
                while (m_Stack.Count > 0)
                {
                    GameObject.Destroy(m_Stack.Pop());
                }
            }
            else
            {
                while (m_Stack.Count > 0)
                {
                    poolFactory?.OnDestroy(m_Stack.Pop());
                }
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }

    internal interface IGameObjectPool<T> : IDisposable where T : Component
    {
        Transform PoolTransform
        {
            get;
        }

        int Capacity
        {
            get;
        }

        int Count
        {
            get;
        }

        T Get();

        T Get(Transform parent);

        T Get(Transform parent, Vector3 position, Quaternion quaternion);

        T Get(Transform parent, Vector3 position, Quaternion quaternion, Vector3 scale);

        void Recycle(T t);

        void Clear();
    }
}
