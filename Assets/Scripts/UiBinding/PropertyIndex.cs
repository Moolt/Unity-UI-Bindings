using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UiBinding.Core
{
    [Serializable]
    public struct PropertyIndex
    {
        [SerializeField] private int _index;

        public PropertyInfo ResolveFrom(object target, PropertyFilter filter)
        {
            var targetType = target.GetType();
            var properties = MemberCollection<PropertyInfo>
                .FilteredMembersFor(targetType, filter)
                .ToArray();
            return properties[_index];
        }

        public static implicit operator int(PropertyIndex index) => index.Index;

        public static implicit operator PropertyIndex(int index) => new PropertyIndex() { Index = index };

        public int Index
        {
            get => _index;
            set => _index = value;
        }
    }
}