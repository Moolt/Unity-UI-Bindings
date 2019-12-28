using System;
using System.Collections.Generic;
using System.Linq;

public class TypeCollection<T>
{
    public TypeCollection()
    {
        Types = Assemblies.Core.GetTypes()
            .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();
        Names = Types.Select(t => t.Name).ToArray();
    }

    public IList<Type> Types { get; }

    public IList<string> Names { get; }

    public int IndexOf(Type type)
    {
        return Math.Max(0, Types.IndexOf(type));
    }

    public int IndexOf(string name)
    {
        return Math.Max(0, Names.IndexOf(name));
    }

    public Type TypeAt(int index) => Types[index];
}
