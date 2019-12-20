using ObjectSerialization;
using System;
using System.Collections.Generic;
using UiBinding.Core;
using UnityEngine;

namespace UiBinding.Conversion
{
    [Serializable]
    public class ConverterIdentifier : Identifier
    {
        [SerializeField] private SerializableDictionary _properties = new SerializableDictionary();

        public IValueConverter ResolveFor(PropertyBinding binding)
        {
            if (IsDefault)
            {
                return new DefaultConverter(binding.SourcePropertyType, binding.TargetPropertyType);
            }

            return Instantiate();
        }

        public static implicit operator string(ConverterIdentifier identifier) => identifier.Name;

        public static implicit operator ConverterIdentifier(string name) => new ConverterIdentifier() { Name = name };

        public bool IsDefault => Name == Default;

        public static string Default => typeof(DefaultConverter).Name;

        public void ClearProperties()
        {
            _properties.Clear();
        }

        public void SetPropertyValue(string property, object value)
        {
            RemoveObsoleteProperties();

            _properties[property] = value;
        }

        public object GetPropertyValue(string property)
        {
            RemoveObsoleteProperties();

            if (!_properties.TryGetValue(property, out var value))
            {
                var defaultValue = DefaultForProperty(property);
                _properties[property] = defaultValue;
                return defaultValue;
            }

            return value;
        }

        public void RemoveObsoleteProperties()
        {
            var obsoleteProperties = new List<object>();

            foreach (var storedProperty in _properties)
            {
                var name = storedProperty.Key as string;
                var propertyInfo = ConverterType.GetProperty(name);

                if (propertyInfo == null)
                {
                    obsoleteProperties.Add(obsoleteProperties);
                }
            }

            foreach (var obsoleteProperty in obsoleteProperties)
            {
                _properties.Remove(obsoleteProperty);
            }
        }

        private IValueConverter Instantiate()
        {
            var instance = (IValueConverter)Activator.CreateInstance(ConverterType);

            // Initialize all properties as defined by the user in the inspector.
            foreach (var storedProperty in _properties)
            {
                var name = storedProperty.Key as string;
                var value = storedProperty.Value;

                var property = ConverterType.GetProperty(name);

                if (property != null)
                {
                    property.SetValue(instance, value);
                }
            }

            return instance;
        }

        private object DefaultForProperty(string property)
        {
            var converterType = ConversionProvider.TypeOfConverterFor(this);
            var propertyInfo = converterType.GetProperty(property);
            var converterInstance = Instantiate();
            var defaultValue = propertyInfo.GetValue(converterInstance);

            if (defaultValue == null)
            {
                var propertyType = propertyInfo.PropertyType;
                return Activator.CreateInstance(propertyType);
            }

            return defaultValue;
        }

        private Type ConverterType => ConversionProvider.TypeOfConverterFor(this);
    }
}