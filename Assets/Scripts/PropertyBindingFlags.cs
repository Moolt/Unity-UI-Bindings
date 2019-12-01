using System.Reflection;

public static class PropertyBindingFlags
{
    public const BindingFlags Source = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
    public const BindingFlags Target = BindingFlags.Public | BindingFlags.Instance;
}