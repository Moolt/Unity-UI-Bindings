using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectSerialization
{
    [Serializable]
    public class SerializableList : IEnumerable<SerializationContainer>
    {
        [SerializeField] private List<SerializationContainer> _serializedObjects = new List<SerializationContainer>();

        public SerializableList(params object[] values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }

        public void Add(object value)
        {
            var container = new SerializationContainer(value);
            _serializedObjects.Add(container);
        }

        public void Remove(object value)
        {
            SerializationContainer container;

            if (value is SerializationContainer)
            {
                container = value as SerializationContainer;
            }
            else
            {
                container = new SerializationContainer(value);
            }

            Remove(container);
        }

        private void Remove(SerializationContainer container)
        {
            _serializedObjects.Remove(container);
        }

        public IEnumerator<SerializationContainer> GetEnumerator() => _serializedObjects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _serializedObjects.GetEnumerator();
    }
}