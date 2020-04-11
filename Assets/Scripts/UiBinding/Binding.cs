using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace UiBinding.Core
{
    public abstract class Binding<TSourceIdentifier, TTargetIdentifier> : MonoBehaviour, IBinding
    {
        [SerializeField] private TSourceIdentifier _sourceIdentifier;
        [SerializeField] private TTargetIdentifier _targetIdentifier;

        [SerializeField] private BindingMemberDefinition _sourceDefinition = new BindingMemberDefinition();
        [SerializeField] private UnityObject _target;

        private INotifyPropertyChanged _source;
        private readonly IList<IDisposable> _destructors = new List<IDisposable>();
        private bool _bound;

        protected virtual void Awake()
        {
            BeforeAwake();
            Guard.AssertNotNull(gameObject, Target, "Binding target missing.");

            if (SourceDefinition.Kind == BindingMemberKind.Type)
            {
                return;
            }

            var instance = SourceDefinition.Instance;
            Guard.AssertNotNull(gameObject, instance, "Binding source missing.");
            Bind(instance);
            AfterAwake();
        }

        protected virtual void BeforeAwake()
        {
        }

        protected virtual void AfterAwake()
        {
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

        public virtual UnityObject Target
        {
            get => _target;
            set => _target = value;
        }

        public bool HasSource => _sourceDefinition.Kind == BindingMemberKind.Type || _sourceDefinition.Instance != null;

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

        public TSourceIdentifier SourceIdentifier
        {
            get => _sourceIdentifier;
            set => _sourceIdentifier = value;
        }

        public TTargetIdentifier TargetIdentifier
        {
            get => _targetIdentifier;
            set => _targetIdentifier = value;
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

        public virtual void ApplyValuesOf(Binding<TSourceIdentifier, TTargetIdentifier> other)
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

        protected abstract void OnEstablishBinding();
    }
}