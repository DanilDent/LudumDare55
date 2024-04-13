using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public interface IMonoObjectPool1<T, TArgs>
       where T : MonoBehaviour
       where TArgs : PoolObjArgs
    {
        T Create(TArgs args, Transform parent);
        void Destroy(T instance);
        void Initialize(IEnumerable<TArgs> elements, Transform transformContainer, Action<T> onInstantiatedCallback = null);
    }
}