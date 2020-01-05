using System;
using System.Collections.Generic;
using System.Linq;

namespace UiBinding.Core
{
    [Serializable]
    public class IndexedIdentifier
    {
        private readonly Identifier _identifier;
        private readonly IList<string> _memberNames;

        private IndexedIdentifier(Identifier identifier, IList<string> memberNames)
        {
            _identifier = identifier;
            _memberNames = memberNames;
        }

        public static IndexedIdentifier For(Identifier identifier, IList<string> memberNames)
        {
            return new IndexedIdentifier(identifier, memberNames);
        }

        public int Index
        {
            get => _memberNames.Contains(_identifier.Name) ? _memberNames.IndexOf(_identifier.Name) : 0;
            set => _identifier.Name = _memberNames.ElementAt(value);
        }
    }
}