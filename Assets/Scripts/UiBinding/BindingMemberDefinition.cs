using System;
using UnityEngine;

namespace UiBinding.Core
{
    [Serializable]
    public class BindingMemberDefinition
    {
        [SerializeField] private BindingMemberKind _kind;
        [SerializeField] private string _type = string.Empty;
        [SerializeField] private BindableMonoBehaviour _instance;

        public BindingMemberKind Kind
        {
            get => _kind;
            set => _kind = value;
        }

        public string TypeName
        {
            get => _type;
            set => _type = value;
        }

        public Type Type
        {
            get => Type.GetType(_type);
            set => _type = value.AssemblyQualifiedName;
        }

        public BindableMonoBehaviour Instance
        {
            get => _instance;
            set => _instance = value;
        }
    }
}