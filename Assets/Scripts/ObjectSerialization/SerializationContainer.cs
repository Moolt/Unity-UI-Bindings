using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectSerialization
{
    [Serializable]
    public class SerializationContainer
    {
        [SerializeField] private string _value;
        [SerializeField] private string _type;

        public SerializationContainer(object value)
        {
            Value = value;
        }

        public object Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public Type StoredType => Type.GetType(_type);

        public T GetValue<T>()
        {
            return (T)Value;
        }

        public bool Is(Type type)
        {
            return StoredType == type;
        }

        public bool Is<T>()
        {
            return Is(typeof(T));
        }

        public static SerializationContainer From(object value)
        {
            return new SerializationContainer(value);
        }

        public override string ToString()
        {
            return $"{_type}: {_value}";
        }

        private void SetValue(object value)
        {
            var type = value.GetType();
            _type = type.AssemblyQualifiedName;

            if (type.IsPrimitive || value is string)
            {
                _value = value.ToString();
                return;
            }

            _value = JsonUtility.ToJson(value);
        }

        private object GetValue()
        {
            if (_value == null)
            {
                return null;
            }

            if (StoredType.IsPrimitive || StoredType == typeof(string))
            {
                return Convert.ChangeType(_value, StoredType);
            }

            return JsonUtility.FromJson(_value, StoredType);
        }

        public override bool Equals(object obj)
        {
            var container = obj as SerializationContainer;
            return container != null &&
                   _value == container._value &&
                   _type == container._type;
        }

        public override int GetHashCode()
        {
            var hashCode = -862051595;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_value);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_type);
            return hashCode;
        }
    }
}