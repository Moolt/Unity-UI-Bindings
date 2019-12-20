using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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

    [Serializable]
    public abstract class Identifier
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _valid = true;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _valid = true;
            }
        }

        public bool Valid
        {
            get => _valid;
            set => _valid = value;
        }

        public void AssertValid(string message)
        {
            if(_valid)
            {
                return;
            }

            throw new Exception(message);
        }
    }

    [Serializable]
    public class MethodIdentifier : Identifier
    {
        public MethodInfo ResolveFrom(object target)
        {
            return target
                .GetType()
                .GetMethods()
                .FirstOrDefault(m => m.Name == Name);
        }
    }
}