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
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
                _instance = null;
        }
    }
}