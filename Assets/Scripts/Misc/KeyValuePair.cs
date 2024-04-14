using System;
using UnityEngine;

namespace Misc
{
    [Serializable]
    public class KeyValuePair<TKey, TValue>
    {
        public KeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public TKey Key { get => _key; set => _key = value; }

        public TValue Value { get => _value; set => _value = value; }

        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;

        public KeyValuePair(KeyValuePair<TKey, TValue> keyValuePair)
        {
            _key = keyValuePair.Key;
            _value = default;
        }
    }
}