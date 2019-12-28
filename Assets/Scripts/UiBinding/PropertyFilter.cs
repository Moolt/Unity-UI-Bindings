using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UiBinding.Core
{
    public class PropertyFilter : MemberFilter<PropertyInfo>
    {
        public PropertyFilter(BindingFlags bindingFlags, Func<PropertyInfo, bool> selector = null)
            : base(bindingFlags, selector)
        {
        }

        public override IEnumerable<PropertyInfo> FilteredMembersFor(Type type)
        {
            return type
                .GetProperties(BindingFlags)
                .Where(Selector)
                .ToList();
        }
    }
}