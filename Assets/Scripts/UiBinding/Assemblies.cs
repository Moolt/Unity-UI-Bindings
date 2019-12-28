using System;
using System.Linq;
using System.Reflection;

public static class Assemblies
{
    public static Assembly Core { get; } = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Assembly-CSharp");
}
