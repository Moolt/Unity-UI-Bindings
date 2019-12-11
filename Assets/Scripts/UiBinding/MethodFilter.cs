using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UiBinding.Core
{
    public class MethodFilter : MemberFilter<MethodInfo>
    {
        public MethodFilter(BindingFlags bindingFlags, Func<MethodInfo, bool> selector = null)
            : base(bindingFlags, selector)
        {
        }

        public override IEnumerable<MethodInfo> FilteredMembersFor(Type type)
        {
            return type
                .GetMethods(BindingFlags)
                .Where(Selector);
        }
    }
}