using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crystal.Res
{
    /// <summary>
    /// Resources 资源加载
    /// </summary>
    public class ResourcesLoader : MonoSingleton<ResourcesLoader>, IResourcesLoader
    {

        private Dictionary<string, GameObject> cachedDict = new Dictionary<string, GameObject>();

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            if (cachedDict != null)
            {
                foreach (var kvp in cachedDict)
                {
                    GameObject.Destroy(kvp.Value);
                }
                cachedDict.Clear();
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="cachedPrefab"></param>
        /// <param name="instantiate"></param>
        /// <returns></returns>
        private T LoadGameObject<T>(string path, bool cachedPrefab, Func<GameObject, GameObject> instantiate) where T : MonoBehaviour
        {
            GameObject go = null;
            if (cachedPrefab)
            {
                cachedDict.TryGetValue(path, out go);
                if (go == null)
                {
                    go = Resources.Load<GameObject>(path);
                    if (go != null)
                    {
                        cachedDict.Add(path, go);
                    }
                }
            }
            else
            {
                go = Resources.Load<GameObject>(path);
            }
            if (go == null)
            {
                return null;
            }
            return InstantiateAndAddComponent<T>(go, instantiate);
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="cachedPrefab"></param>
        /// <param name="instantiate"></param>
        /// <param name="loadResult"></param>
        private void LoadGameObjectAsync<T>(string path, bool cachedPrefab, Func<GameObject, GameObject> instantiate, Action<T> loadResult) where T : MonoBehaviour
        {
            if (cachedPrefab)
            {
                GameObject go = null;
                cachedDict.TryGetValue(path, out go);
                if (go == null)
                {
                    var request = Resources.LoadAsync<GameObject>(path);
                    request.completed += (resultRequest) =>
                    {
                        go = request.asset as GameObject;
                        if (go != null)
                        {
                            cachedDict.Add(path, go);
                            var t = InstantiateAndAddComponent<T>(go, instantiate);
                            loadResult?.Invoke(t);
                        }
                    };
                }
                else
                {
                    var t = InstantiateAndAddComponent<T>(go, instantiate);
                    loadResult?.Invoke(t);
                }
            }
            else
            {
                var request = Resources.LoadAsync<GameObject>(path);
                request.completed += (resultRequest) =>
                {
                    var go = request.asset as GameObject;
                    if (go != null)
                    {
                        var t = InstantiateAndAddComponent<T>(go, instantiate);
                        loadResult?.Invoke(t);
                    }
                };
            }
        }

        /// <summary>
        /// 实例化并添加脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="instantiate"></param>
        /// <returns></returns>
        private T InstantiateAndAddComponent<T>(GameObject go, Func<GameObject, GameObject> instantiate) where T : MonoBehaviour
        {
            var instance = instantiate(go);
            var t = instance.GetComponent<T>();
            if (t == null)
            {
                t = instance.AddComponent<T>();
            }
            return t;
        }

        /// <summary>
        /// 加载Resources 目录下 GameObject并且添加 T(MonoBehaviour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="cachedPrefab"></param>
        /// <returns></returns>
        public T Load<T>(string path, Transform parent = null, bool cachedPrefab = true) where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            return LoadGameObject<T>(path, cachedPrefab, (prefabGo) => Instantiate<GameObject>(prefabGo));
        }

        /// <summary>
        /// 加载Resources 目录下 GameObject并且添加 T(MonoBehaviour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="cachedPrefab"></param>
        /// <returns></returns>
        public T Load<T>(string path, Transform parent, Vector3 position, Quaternion rotation, bool cachedPrefab = true) where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            return LoadGameObject<T>(path, cachedPrefab, (prefabGo) => Instantiate<GameObject>(prefabGo, position, rotation, parent));
        }

        /// <summary>
        /// 加载Resources 目录下 GameObject并且添加 T(MonoBehaviour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="cachedPrefab"></param>
        /// <returns></returns>
        public T Load<T>(string path, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale, bool cachedPrefab = true) where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            return LoadGameObject<T>(path, cachedPrefab, (prefabGo) =>
            {
                var instance = Instantiate<GameObject>(prefabGo, position, rotation, parent);
                if (instance != null)
                {
                    instance.transform.localScale = scale;
                }
                return instance;
            });
        }

        /// <summary>
        /// 异步加载Resources 目录下 GameObject并且添加 T(MonoBehaviour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="loadResult"></param>
        /// <param name="cachedPrefab"></param>
        public void LoadAsync<T>(string path, Transform parent, Action<T> loadResult, bool cachedPrefab = true) where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            LoadGameObjectAsync<T>(path, cachedPrefab, (prefabGo) => Instantiate<GameObject>(prefabGo, parent), loadResult);
        }

        /// <summary>
        /// 异步加载Resources 目录下 GameObject并且添加 T(MonoBehaviour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="loadResult"></param>
        /// <param name="cachedPrefab"></param>
        public void LoadAsync<T>(string path, Transform parent, Vector3 position, Quaternion rotation, Action<T> loadResult, bool cachedPrefab = true) where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            LoadGameObjectAsync<T>(path, cachedPrefab, (prefabGo) => Instantiate<GameObject>(prefabGo, position, rotation, parent), loadResult);
        }

        /// <summary>
        /// 异步加载Resources 目录下 GameObject并且添加 T(MonoBehaviour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="loadResult"></param>
        /// <param name="cachedPrefab"></param>
        public void LoadAsync<T>(string path, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale, Action<T> loadResult, bool cachedPrefab = true) where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            LoadGameObjectAsync<T>(path, cachedPrefab, (prefabGo) =>
            {
                var t = Instantiate<GameObject>(prefabGo, position, rotation, parent);
                if (t != null)
                {
                    t.transform.localScale = scale;
                }
                return t;
            }, loadResult);
        }


        protected override void OnWillDestroy()
        {
            cachedDict.Clear();
            cachedDict = null;
        }

        /// <summary>
        /// 加载Resources目录下资源
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public E Load<E>(string path) where E : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            return Resources.Load<E>(path);
        }

        /// <summary>
        /// 异步加载Resources目录下资源
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="path"></param>
        /// <param name="loadResult"></param>
        public void LoadAsync<E>(string path, Action<E> loadResult) where E : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path could not be null.");
            }
            var request = Resources.LoadAsync<E>(path);
            request.completed += (result) =>
            {
                loadResult?.Invoke(request.asset as E);
            };
        }
    }

    internal interface IResourcesLoader
    {
        E Load<E>(string path) where E : UnityEngine.Object;

        void LoadAsync<E>(string path, Action<E> loadResult) where E : UnityEngine.Object;

        T Load<T>(string path, Transform parent, bool cachedPrefab) where T : MonoBehaviour;

        T Load<T>(string path, Transform parent, Vector3 position, Quaternion roatation, bool cachedPrefab) where T : MonoBehaviour;

        T Load<T>(string path, Transform parent, Vector3 position, Quaternion roatation, Vector3 scale, bool cachedPrefab) where T : MonoBehaviour;

        void LoadAsync<T>(string path, Transform parent, Action<T> loadResult, bool cachedPrefab) where T : MonoBehaviour;

        void LoadAsync<T>(string path, Transform parent, Vector3 position, Quaternion roatation, Action<T> loadResult, bool cachedPrefab) where T : MonoBehaviour;

        void LoadAsync<T>(string path, Transform parent, Vector3 position, Quaternion roatation, Vector3 scale, Action<T> loadResult, bool cachedPrefab) where T : MonoBehaviour;

        void ClearCache();
    }
}
