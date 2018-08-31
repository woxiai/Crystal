using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crystal
{
    /// <summary>
    /// MonoBehaviour单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> 
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = GameObject.FindObjectOfType<T>();
                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject(string.Format("[Singleton][{0}]", typeof(T).Name), typeof(T)).GetComponent<T>();
                        m_Instance.OnInitialize();
                    }
                }
                return m_Instance;
            }
        }

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnWillDestroy()
        {
        }

        protected virtual void OnDestroy() {
            OnWillDestroy();
            m_Instance = null;
        }
    }
}
