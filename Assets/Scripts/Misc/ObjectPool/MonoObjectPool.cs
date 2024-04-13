using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class MonoObjectPool<T, TArgs>
            where T : MonoBehaviour
            where TArgs : PoolObjArgs
    {
        public GameObject Prefab => _factory.Prefab;

        private PoolObjectFactory<T, TArgs> _factory;
        private Dictionary<string, Queue<T>> _queues;
        private Transform _transformContainer;

        public MonoObjectPool(PoolObjectFactory<T, TArgs> factory)
        {
            _factory = factory;
            _queues = new Dictionary<string, Queue<T>>();
        }

        public void Initialize(IEnumerable<TArgs> elements, Transform transformContainer, Action<T> onInstantiatedCallback = null)
        {
            _transformContainer = transformContainer;

            foreach (var element in elements)
            {
                if (!_queues.ContainsKey(element.Key))
                {
                    _queues.Add(element.Key, new Queue<T>());
                }

                T instance = Instantiate(element);
                instance.gameObject.name = $"{Prefab.name}_{element.Key}";
                onInstantiatedCallback?.Invoke(instance);
                instance.gameObject.SetActive(false);
                _queues[element.Key].Enqueue(instance);
            }
        }

        public bool IsQueueCreated(string key)
        {
            return _queues.ContainsKey(key);
        }

        public T Create(string key, Transform parent, TArgs args = null)
        {
            if (!IsQueueCreated(key) || _queues[key].Count == 0)
            {
                _queues.Add(key, new Queue<T>());

                T newInstance = Instantiate(args);
                newInstance.gameObject.name = $"{Prefab.name}_{key}";
                newInstance.gameObject.transform.SetParent(parent);
                newInstance.gameObject.SetActive(true);
                _queues[key].Enqueue(newInstance);

                return newInstance;
            }

            T instance = _queues[key].Dequeue();
            instance.transform.SetParent(parent);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Destroy(string key, T instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_transformContainer);
            _queues[key].Enqueue(instance);
        }

        public void DestroyCompletely(string key, T instance)
        {
            _queues.Remove(key);
            UnityEngine.Object.Destroy(instance.gameObject);
        }

        private T Instantiate(TArgs args)
        {
            T instance = _factory.Create(args);
            instance.transform.SetParent(_transformContainer);
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}
