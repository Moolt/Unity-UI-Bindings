using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UiBinding.Core
{
    [Serializable]
    public struct MethodIndex
    {
        [SerializeField] private int _index;

        public MethodInfo ResolveFrom(object target, MethodFilter filter)
        {
            var targetType = target.GetType();
            var methods = MemberCollection<MethodInfo>
                .FilteredMembersFor(targetType, filter)
                .ToArray();
            return methods[_index];
        }

        public static implicit operator int(MethodIndex index) => index.Index;

        public static implicit operator MethodIndex(int index) => new MethodIndex() { Index = index };

        public int Index
        {
            get => _index;
            set => _index = value;
        }
    }
}