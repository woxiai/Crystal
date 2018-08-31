using System;
using UnityEngine;
using Crystal.Res;

namespace Crystal.Pool
{
    public class PrefabGameObjectPoolFactory<T> : IGameObjectPoolFactory<T> where T : MonoBehaviour
    {
        public void OnRecycle(T t)
        {
            if (t != null)
            {
                t.transform.localPosition = Vector3.zero;
                t.enabled = false;
            }
        }

        public void OnDestroy(T t)
        {
            if (t != null)
            {
                GameObject.Destroy(t);
            }
        }

        public void Dispose()
        {
        }

        private string GetPrefabPath<E>() where E : T
        {
            var pas = typeof(T).GetCustomAttributes(typeof(PrefabAttribute), false);
            if (pas != null && pas.Length > 0)
            {
                var pa = pas[0] as PrefabAttribute;
                if (pa != null)
                {
                    return pa.Path;
                }
            }
            return string.Empty;
        }

        public T Instaniate<E>(Transform parent) where E : T
        {
            var path = GetPrefabPath<T>();
            if (!string.IsNullOrEmpty(path))
            {
                return ResourcesLoader.Instance.Load<T>(path, parent);
            }
            return default(T);
        }

        public T Instaniate<E>(Transform parent, Vector3 position, Quaternion rotation) where E : T
        {
            var path = GetPrefabPath<T>();
            if (!string.IsNullOrEmpty(path))
            {
                return ResourcesLoader.Instance.Load<T>(path, parent, position, rotation);
            }
            return default(T);
        }

        public T Instaniate<E>(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale) where E : T
        {
            var path = GetPrefabPath<T>();
            if (!string.IsNullOrEmpty(path))
            {
                return ResourcesLoader.Instance.Load<T>(path, parent, position, rotation, scale);
            }
            return default(T);
        }

        public void OnGet(T t)
        {
            if (t != null)
            {
                t.enabled = true;
            }
        }

        public void OnGet(T t, Transform parent)
        {
            if (t != null)
            {
                t.enabled = true;
                t.transform.SetParent(parent);
            }
        }

        public void OnGet(T t, Transform parent, Vector3 position, Quaternion rotation)
        {
            if (t != null)
            {
                t.enabled = true;
                var trans = t.transform;
                trans.SetParent(parent);
                trans.localPosition = position;
                trans.localRotation = rotation;
            }
        }

        public void OnGet(T t, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (t != null)
            {
                t.enabled = true;
                var trans = t.transform;
                trans.SetParent(parent);
                trans.localPosition = position;
                trans.rotation = rotation;
                trans.localScale = scale;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PrefabAttribute : Attribute
    {

        public string Path
        {
            set;
            get;
        }
    }
}
