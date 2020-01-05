using System;
using System.Linq;
using System.Reflection;

namespace UiBinding.Core
{
    [Serializable]
    public class MethodIdentifier : Identifier
    {
        public MethodInfo ResolveFrom(object target)
        {
            return target
                .GetType()
                .GetMethods()
                .FirstOrDefault(m => m.Name == Name);
        }
    }
}