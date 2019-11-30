using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

    public static IValueConverter Instantiate(ConverterIndex converter)
    {
        var converterType = AvailableConverters.ElementAt(converter);
        return (IValueConverter)Activator.CreateInstance(converterType);
    }

    private static Assembly Assembly => AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Assembly-CSharp");
}