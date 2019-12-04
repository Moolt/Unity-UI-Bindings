using System;
using System.Collections.Generic;
using System.Reflection;

namespace UiBinding.Core
{
    public class MethodCollection : MemberCollection<MethodInfo>
    {
        private MethodCollection(Type type) : base(type)
        {
        }

        public static MethodCollection For(Type type)
        {
            return new MethodCollection(type);
        }

        protected override IEnumerable<MethodInfo> GetMembers()
        {
            return MemberType.GetMethods(BindingFlags);
        }
    }
}