using System;
using System.ComponentModel;
using UnityObject = UnityEngine.Object;

namespace UiBinding.Core
{
    public interface IBinding
    {
        INotifyPropertyChanged Source { get; set; }

        UnityObject Target { get; set; }

        Type SourceType { get; }

        Type TargetType { get; }

        void Bind(INotifyPropertyChanged source);

        BindingMemberDefinition SourceDefinition { get; }
    }
}