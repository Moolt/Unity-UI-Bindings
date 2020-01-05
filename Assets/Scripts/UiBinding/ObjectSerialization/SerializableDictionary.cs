using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectSerialization
{
    [Serializable]
    public class SerializableDictionary : IDictionary<object, object>
    {
        [SerializeField] private List<SerializationContainer> _keys = new List<SerializationContainer>();
        [SerializeField] private List<SerializationContainer> _values = new List<SerializationContainer>();

        public object this[object key]
        {
            get
            {
                if (!TryGetValue(key, out var value))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }
            set
            {
                if (ContainsKey(key))
                {
                    var index = _keys.IndexOf(Serialize(key));
                    _values.RemoveAt(index);
                    _values.Insert(index, Serialize(value));
                    return;
                }

                Add(key, value);
            }
        }

        public ICollection<object> Keys => _keys.Select(k => k.Value).ToList();

        public ICollection<object> Values => _values.Select(v => v.Value).ToList();

        public int Count => _keys.Count;

        public bool IsReadOnly => false;

        public void Add(object key, object value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} already exists.");
            }

            var serializedKey = SerializationContainer.From(key);
            var serializedValue = SerializationContainer.From(value);

            _keys.Add(serializedKey);
            _values.Add(serializedValue);
        }

        public void Add(KeyValuePair<object, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public bool Contains(KeyValuePair<object, object> item)
        {
            if (!TryGetValue(item.Key, out var value))
            {
                return false;
            }

            return item.Value.Equals(value);
        }

        public bool ContainsKey(object key)
        {
            return _keys.Contains(Serialize(key));
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = arrayIndex; i < array.Length; i++)
            {
                var pair = array[i];
                this[pair.Key] = pair.Value;
            }
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            for (var i = 0; i < _values.Count; i++)
            {
                var key = _keys[i].Value;
                var value = _values[i].Value;

                yield return new KeyValuePair<object, object>(key, value);
            }
        }

        public bool Remove(object key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }

            var index = _keys.IndexOf(Serialize(key));

            _keys.RemoveAt(index);
            _values.RemoveAt(index);
            return true;
        }

        public bool Remove(KeyValuePair<object, object> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(object key, out object value)
        {
            if (!ContainsKey(key))
            {
                value = null;
                return false;
            }

            var index = _keys.IndexOf(Serialize(key));
            value = _values[index].Value;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private SerializationContainer Serialize(object value)
        {
            return SerializationContainer.From(value);
        }
    }
}