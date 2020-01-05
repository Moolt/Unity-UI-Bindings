using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace UiBinding.Core
{
    [Serializable]
    public enum BindingMemberKind
    {
        Instance,
        Type,
    }

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

    public interface IBinding
    {
        INotifyPropertyChanged Source { get; set; }

        UnityObject Target { get; set; }

        Type SourceType { get; }

        Type TargetType { get; }

        void Bind(INotifyPropertyChanged source);
    }

    public abstract class Binding<TSourceIndex, TTargetIndex> : MonoBehaviour, IBinding
    {
        [SerializeField] private TSourceIndex _sourceIndex;
        [SerializeField] private TTargetIndex _targetIndex;

        [SerializeField] private BindingMemberDefinition _sourceDefinition = new BindingMemberDefinition();
        [SerializeField] private UnityObject _target;

        private INotifyPropertyChanged _source;
        private readonly IList<IDisposable> _destructors = new List<IDisposable>();
        private bool _bound;

        protected virtual void Awake()
        {
            AssertNotNull(Target, "Binding target missing.");

            if (SourceDefinition.Kind == BindingMemberKind.Type)
            {
                return;
            }

            var instance = SourceDefinition.Instance;
            AssertNotNull(instance, "Binding source missing.");
            Bind(instance);
        }

        private void OnDestroy()
        {
            Break();
        }

        public BindingMemberDefinition SourceDefinition
        {
            get => _sourceDefinition;
            private set => _sourceDefinition = value;
        }

        public INotifyPropertyChanged Source
        {
            get => _sourceDefinition.Kind == BindingMemberKind.Instance ? _sourceDefinition.Instance : _source;
            set
            {
                if (_sourceDefinition.Kind == BindingMemberKind.Instance)
                {
                    _sourceDefinition.Instance = value as BindableMonoBehaviour;
                    return;
                }

                _source = value;
            }
        }

        public UnityObject Target
        {
            get => _target;
            set => _target = value;
        }

        public bool HasSource => _sourceDefinition != null;

        public bool HasTarget => _target != null;

        public bool HasSourceAndTarget => HasSource && HasTarget;

        public Type SourceType
        {
            get
            {
                switch (_sourceDefinition.Kind)
                {
                    case BindingMemberKind.Type: return _sourceDefinition.Type;
                    default: return _sourceDefinition.Instance?.GetType();
                }
            }
        }

        public Type TargetType => _target?.GetType();

        public TSourceIndex SourceIdentifier
        {
            get => _sourceIndex;
            set => _sourceIndex = value;
        }

        public TTargetIndex TargetIdentifier
        {
            get => _targetIndex;
            set => _targetIndex = value;
        }

        public virtual void Bind(INotifyPropertyChanged source)
        {
            if (_bound)
            {
                throw new Exception("The binding has already been established.");
            }

            _bound = true;
            Source = source;

            OnEstablishBinding();
        }

        public void Break()
        {
            foreach (var destructor in _destructors)
            {
                destructor?.Dispose();
            }

            _destructors.Clear();
        }

        public virtual void ApplyValuesOf(Binding<TSourceIndex, TTargetIndex> other)
        {
            SourceIdentifier = other.SourceIdentifier;
            TargetIdentifier = other.TargetIdentifier;
            SourceDefinition = other.SourceDefinition;
            Target = other.Target;
        }

        protected void AddDestructor(IDisposable disposable)
        {
            _destructors.Add(disposable);
        }

        protected void AssertNotNull(object target, string message)
        {
            if (target is UnityObject unityObject && unityObject != null)
            {
                return;
            }

            if (target != null)
            {
                return;
            }

            throw new Exception($"{gameObject.name}: {message}");
        }

        protected abstract void OnEstablishBinding();
    }
}