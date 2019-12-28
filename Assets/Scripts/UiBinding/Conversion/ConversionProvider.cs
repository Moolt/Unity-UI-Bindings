using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UiBinding.Core;

namespace UiBinding.Conversion
{
    public static class ConversionProvider
    {
        public static IEnumerable<Type> AvailableConverters
        {
            get
            {
                var converters = Assemblies.Core
                    .GetTypes()
                    .Where(t => typeof(IValueConverter).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t.HasDefaultConstructor())
                    .ToList();
                converters.Insert(0, typeof(DefaultConverter));
                return converters;
            }
        }

        public static IEnumerable<string> AvailableConverterNames => AvailableConverters.Select(c => c.Name);

        public static IEnumerable<PropertyInfo> PropertiesFor(ConverterIdentifier identifier)
        {
            if (identifier.IsDefault)
            {
                return Array.Empty<PropertyInfo>();
            }

            var converterType = TypeOfConverterFor(identifier);
            return converterType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static Type TypeOfConverterFor(ConverterIdentifier identifier)
        {
            return AvailableConverters.FirstOrDefault(c => c.Name == identifier);
        }        
    }
}