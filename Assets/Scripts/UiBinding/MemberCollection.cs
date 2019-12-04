using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UiBinding.Core
{
    public abstract class MemberCollection<TMember> : List<TMember>
    where TMember : MemberInfo
    {
        private Type _type;
        private BindingFlags _bindingFlags;

        protected MemberCollection(Type type)
        {
            MemberType = type;
            Refresh();
        }

        public MemberCollection<TMember> WithBindingFlags(BindingFlags bindingFlags)
        {
            BindingFlags = bindingFlags;
            Refresh();
            return this;
        }

        public MemberCollection<TMember> Refresh()
        {
            if (MemberType == null)
            {
                return this;
            }

            Clear();

            var members = GetMembers();
            AddRange(members);
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
            return MemberType == type;
        }

        public MemberCollection<TMember> ChangeTargetTypeIfNecessary(Type type)
        {
            if (Targets(type))
            {
                return this;
            }

            return ChangeTargetType(type);
        }

        public MemberCollection<TMember> ChangeTargetType(Type type)
        {
            MemberType = type;
            Refresh();
            return this;
        }

        public string[] Names => this.Select(p => p.Name).ToArray();

        protected abstract IEnumerable<TMember> GetMembers();

        protected Type MemberType
        {
            get => _type;
            set => _type = value;
        }

        protected BindingFlags BindingFlags
        {
            get => _bindingFlags;
            set => _bindingFlags = value;
        }
    }
}