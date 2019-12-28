using System.Collections;
using System.Reflection;
using UnityEngine.Events;

namespace UiBinding.Core
{
    public static class MemberFilters
    {
        private static class MemberBindingFlags
        {
            public const BindingFlags Source = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

            public const BindingFlags Target = BindingFlags.Public | BindingFlags.Instance;
        }

        public static PropertyFilter SourceProperties { get; } = new PropertyFilter(MemberBindingFlags.Source);

        public static PropertyFilter TargetProperties { get; } = new PropertyFilter(MemberBindingFlags.Target);

        public static MethodFilter SourceCallbacks { get; } = new MethodFilter(MemberBindingFlags.Source, m => !m.IsSpecialName);

        public static PropertyFilter TargetEvents { get; } = new PropertyFilter(MemberBindingFlags.Target, m => typeof(UnityEventBase).IsAssignableFrom(m.PropertyType));

        public static PropertyFilter Lists { get; } = new PropertyFilter(MemberBindingFlags.Source, m => typeof(IEnumerable).IsAssignableFrom(m.PropertyType));
    }
}