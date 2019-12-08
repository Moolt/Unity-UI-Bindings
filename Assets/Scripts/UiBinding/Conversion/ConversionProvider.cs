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
                return Assembly
                    .GetTypes()
                    .Where(t => typeof(IValueConverter).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t.HasDefaultConstructor());
            }
        }

        public static IEnumerable<string> AvailableConverterNames => AvailableConverters.Select(c => c.Name.Replace("Converter", string.Empty));

        public static IEnumerable<PropertyInfo> PropertiesFor(ConverterIndex converter)
        {
            if (converter.IsDefault)
            {
                return Array.Empty<PropertyInfo>();
            }

            var converterType = AvailableConverters.ElementAt(converter);
            return converterType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static Type TypeOfConverterAt(ConverterIndex index)
        {
            return AvailableConverters.ElementAt(index);
        }

        private static Assembly Assembly => AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Assembly-CSharp");
    }
}