using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crystal.Pool
{
    public interface IGameObjectPoolFactory<T> : IDisposable where T : Component
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <returns></returns>
        T Instaniate<E>(Transform parent) where E : T;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <returns></returns>
        T Instaniate<E>(Transform parent, Vector3 position, Quaternion rotation) where E : T;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <returns></returns>
        T Instaniate<E>(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale) where E : T;

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="t"></param>
        void OnGet(T t);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="t"></param>
        /// <param name="parent"></param>
        void OnGet(T t, Transform parent);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="t"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        void OnGet(T t, Transform parent, Vector3 position, Quaternion rotation);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="t"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        void OnGet(T t, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale);

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="t"></param>
        void OnRecycle(T t);

        /// <summary>
        /// 消除
        /// </summary>
        /// <param name="t"></param>
        void OnDestroy(T t);
    }
}
