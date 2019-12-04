using System;
using System.Collections.Generic;
using System.Reflection;

namespace UiBinding.Core
{
    public class PropertyCollection : MemberCollection<PropertyInfo>
    {
        private PropertyCollection(Type type) : base(type)
        {
        }

        public static PropertyCollection For(Type type)
        {
            return new PropertyCollection(type);
        }

        protected override IEnumerable<PropertyInfo> GetMembers()
        {
            return MemberType.GetProperties(BindingFlags);
        }
    }
}