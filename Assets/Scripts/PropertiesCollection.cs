using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class PropertiesCollection : List<PropertyInfo>
{
    private Type _type;
    private BindingFlags _bindingFlags = PropertyBindingFlags.Default;

    private PropertiesCollection(Type type)
    {
        _type = type;
        Refresh();
    }

    public static PropertiesCollection For(Type type)
    {
        return new PropertiesCollection(type);
    }

    public PropertiesCollection WithBindingFlags(BindingFlags bindingFlags)
    {
        _bindingFlags = bindingFlags;
        Refresh();
        return this;
    }    

    public PropertiesCollection Refresh()
    {
        if (_type == null)
        {
            return this;
        }

        Clear();

        var properties = _type.GetProperties(_bindingFlags);
        AddRange(properties);
        return this;
    }

    public new int IndexOf(PropertyInfo property)
    {
        var propertyNames = PropertyNames;
        for (int i = 0; i < Count; i++)
        {
            if (propertyNames[i] == property?.Name)
            {
                return i;
            }
        }

        return 0;
    }

    public bool Targets(Type type)
    {
        return _type == type;
    }

    public PropertiesCollection ChangeTargetTypeIfNecessary(Type type)
    {
        if (Targets(type))
        {
            return this;
        }

        return ChangeTargetType(type);
    }

    public PropertiesCollection ChangeTargetType(Type type)
    {
        _type = type;
        Refresh();
        return this;
    }

    public string[] PropertyNames => this.Select(p => p.Name).ToArray();

    private bool AreEqual(PropertyInfo first, PropertyInfo second)
    {
        return first?.Name == second?.Name && first?.DeclaringType == second?.DeclaringType;
    }
}