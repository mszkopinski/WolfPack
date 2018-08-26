using UnityEngine;

namespace Wolfpack
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static readonly object locker = new object();

        public static T Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            var go = new GameObject { name = $"(singleton) {typeof(T)}" };
                            _instance = go.AddComponent<T>();
                            DontDestroyOnLoad(go);
                        }
                    }

                    return _instance;
                }
            }
        }

        static T _instance;
    }
}
