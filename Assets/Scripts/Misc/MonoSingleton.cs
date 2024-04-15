using UnityEngine;

namespace Misc
{
    public class MonoSingleton<T> : MonoBehaviour
where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<T>();
                if (_instance == null)
                {
                    Debug.LogError($"Error: no singleton component of type {typeof(T)} found in scene");
                }
            }
            else
            {
                Debug.LogError($"Error: two singletons of type {gameObject.name}");
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
                _instance = null;
        }
    }
}