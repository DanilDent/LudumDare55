using System;
using UnityEngine;

namespace CityBuilder
{
    [Serializable]
    public struct KeyValuePair<TKey, TValue>
    {
        public KeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public TKey Key => _key;

        public TValue Value => _value;

        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;
    }
}