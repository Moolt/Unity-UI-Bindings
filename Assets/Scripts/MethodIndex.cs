using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public struct MethodIndex
{
    [SerializeField] private int _index;

    public MethodInfo ResolveFrom(object target, BindingFlags bindingFlags)
    {
        var targetType = target.GetType();
        var properties = targetType.GetMethods(bindingFlags);
        return properties[_index];
    }

    public static implicit operator int(MethodIndex index) => index.Index;

    public static implicit operator MethodIndex(int index) => new MethodIndex() { Index = index };

    public int Index
    {
        get => _index;
        set => _index = value;
    }
}