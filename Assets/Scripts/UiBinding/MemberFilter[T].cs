using System;
using System.Collections.Generic;
using System.Reflection;

namespace UiBinding.Core
{
    public abstract class MemberFilter<TMember>
    where TMember : MemberInfo
    {
        public MemberFilter(BindingFlags bindingFlags, Func<TMember, bool> selector = null)
        {
            BindingFlags = bindingFlags;
            Selector = selector ?? new Func<TMember, bool>(_ => true);
        }

        protected BindingFlags BindingFlags { get; }

        protected Func<TMember, bool> Selector { get; }

        public abstract IEnumerable<TMember> FilteredMembersFor(Type type);
    }
}