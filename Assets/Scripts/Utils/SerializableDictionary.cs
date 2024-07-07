namespace Utils
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> _list = new List<SerializableKeyValuePair<TKey, TValue>>();

        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public void OnBeforeSerialize()
        {
            _list.Clear();
            foreach (var kvp in _dictionary)
            {
                _list.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            foreach (var kvp in _list)
            {
                _dictionary[kvp.Key] = kvp.Value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary[key] = value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys => _dictionary.Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values => _dictionary.Values;
        public int Count => _dictionary.Count;
    }

}