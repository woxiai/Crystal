using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crystal;

namespace Crystal.Pool
{

    public class PoolManager : MonoSingleton<PoolManager>, IPoolManager
    {
        /// <summary>
        /// GameObjects' Pool 字典
        /// </summary>
        private Dictionary<Type, object> gameObjectPoolDict = new Dictionary<Type, object>();

        /// <summary>
        /// Objects' Pool 字典
        /// </summary>
        private Dictionary<Type, object> objectPoolDict = new Dictionary<Type, object>();

        public GameObjectPool<T> CreateGameObjectPool<T>(Transform poolParentTrans, IGameObjectPoolFactory<T> poolFactory, int capacity) where T : Component
        {
            object pool = null;
            var type = typeof(T);
            gameObjectPoolDict.TryGetValue(type, out pool);
            if (pool == null)
            {
                GameObjectPool<T> poolInner = null;
                if (poolParentTrans == null)
                {
                    var trans = new GameObject(type.Name).transform;
                    trans.parent = transform;
                    trans.localPosition = Vector3.one * 10000F;
                    poolInner = new GameObjectPool<T>(trans, poolFactory, capacity);
                }
                else
                {
                    poolInner = new GameObjectPool<T>(poolParentTrans, poolFactory, capacity);
                }
                gameObjectPoolDict.Add(type, poolInner);
                return poolInner;
            }
            else
            {
                return pool as GameObjectPool<T>;
            }
        }

        public ObjectPool<E> CreateObjectPool<E>(int capacity) where E : new()
        {
            object pool = null;
            var type = typeof(E);
            objectPoolDict.TryGetValue(type, out pool);
            if (pool == null)
            {
                var op = new ObjectPool<E>(capacity);
                objectPoolDict.Add(type, op);
                return op;
            }
            return pool as ObjectPool<E>;
        }

        public void DestroyGameObjectPool<T>() where T : Component
        {
            var pool = GetGameObjectPool<T>();
            if (pool != null)
            {
                gameObjectPoolDict.Remove(typeof(T));
                pool.Clear();
                pool = null;
            }
        }

        public void DestroyObjectPool<E>() where E : new()
        {
            var pool = GetObjectPool<E>();
            if (pool != null)
            {
                objectPoolDict.Remove(typeof(E));
                pool.Clear();
                pool = null;
            }
        }

        public GameObjectPool<T> GetGameObjectPool<T>() where T : Component
        {
            object pool = null;
            gameObjectPoolDict.TryGetValue(typeof(T), out pool);
            return pool as GameObjectPool<T>;
        }

        public ObjectPool<E> GetObjectPool<E>() where E : new()
        {
            object pool = null;
            objectPoolDict.TryGetValue(typeof(E), out pool);
            return pool as ObjectPool<E>;
        }
    }

    internal interface IPoolManager
    {
        /// <summary>
        /// 创建 GameObject's Pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolParentTrans"></param>
        /// <param name="poolFactory"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        GameObjectPool<T> CreateGameObjectPool<T>(Transform poolParentTrans, IGameObjectPoolFactory<T> poolFactory, int capacity) where T : Component;

        /// <summary>
        /// 获取 GameObject's Pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        GameObjectPool<T> GetGameObjectPool<T>() where T : Component;

        /// <summary>
        /// 删除 GameObject's Pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void DestroyGameObjectPool<T>() where T : Component;

        /// <summary>
        /// 创建 Object's Pool
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="capacity"></param>
        /// <returns></returns>
        ObjectPool<E> CreateObjectPool<E>(int capacity) where E : new();

        /// <summary>
        /// 获取 Object's Pool
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        ObjectPool<E> GetObjectPool<E>() where E : new();

        /// <summary>
        /// 销毁 Object's Pool
        /// </summary>
        /// <typeparam name="E"></typeparam>
        void DestroyObjectPool<E>() where E : new();
    }
}