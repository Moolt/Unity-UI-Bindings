using System;
using System.Linq;
using System.Reflection;

namespace UiBinding.Core
{
    [Serializable]
    public class PropertyIdentifier : Identifier
    {
        public PropertyInfo ResolveFrom(object target)
        {
            return target
                .GetType()
                .GetProperties()
                .FirstOrDefault(m => m.Name == Name);
        }
    }
}