using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UiBinding.Core
{
    public class MemberCollection<TMember> : List<TMember>
        where TMember : MemberInfo
    {
        private Type _targetType;
        private readonly MemberFilter<TMember> _filter;

        public MemberCollection(Type type, MemberFilter<TMember> filter)
        {
            _targetType = type;
            _filter = filter;
            Refresh();
        }

        public static IEnumerable<TMember> FilteredMembersFor(Type type, MemberFilter<TMember> filter)
        {
            return new MemberCollection<TMember>(type, filter).Members;
        }

        public MemberCollection<TMember> Refresh()
        {
            if (_targetType == null)
            {
                return this;
            }

            Clear();
            AddRange(Members);
            return this;
        }

        public new int IndexOf(TMember member)
        {
            var memberNames = Names;
            for (int i = 0; i < Count; i++)
            {
                if (memberNames[i] == member?.Name)
                {
                    return i;
                }
            }

            return 0;
        }

        public bool Targets(Type type)
        {
            return _targetType == type;
        }

        public bool ChangeTargetTypeIfNecessary(Type type)
        {
            if (Targets(type))
            {
                return false;
            }

            ChangeTargetType(type);
            return true;
        }

        public void ChangeTargetType(Type type)
        {
            _targetType = type;
            Refresh();
        }

        public string[] Names => this.Select(p => p.Name).ToArray();

        protected IEnumerable<TMember> Members => _filter.FilteredMembersFor(_targetType);
    }
}