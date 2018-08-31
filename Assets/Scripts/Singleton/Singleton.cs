using System;
using System.Collections.Generic;

namespace Crystal
{
    /// <summary>
    /// 普通Class的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : Singleton<T>
    {
        private static readonly object lockObj = new object();

        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (lockObj)
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = Activator.CreateInstance<T>();
                            m_Instance.OnInitialize();
                        }
                    }
                }
                return m_Instance;
            }
        }

        protected virtual void OnInitialize()
        {
        }
    }
}
