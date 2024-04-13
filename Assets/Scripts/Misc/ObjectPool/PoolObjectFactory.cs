using UnityEngine;

namespace Misc
{
    public class PoolObjectFactory<T, TArgs>
        where T : MonoBehaviour
        where TArgs : PoolObjArgs
    {
        public GameObject Prefab => _prefab;

        protected GameObject _prefab;

        public PoolObjectFactory(GameObject prefab)
        {
            _prefab = prefab;
        }

        public virtual T Create(TArgs args)
        {
            return GameObject.Instantiate(_prefab).GetComponent<T>();
        }
    }
}
