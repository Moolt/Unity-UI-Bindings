using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public struct PropertyIndex
{
    [SerializeField] private int _index;

    public PropertyInfo ResolveFrom(object target)
    {
        var targetType = target.GetType();
        var properties = targetType.GetProperties(PropertyBindingFlags.Default);
        return properties[_index];
    }

    public static implicit operator int(PropertyIndex index) => index.Index;

    public static implicit operator PropertyIndex(int index) => new PropertyIndex() { Index = index };

    public int Index
    {
        get => _index;
        set => _index = value;
    }
}