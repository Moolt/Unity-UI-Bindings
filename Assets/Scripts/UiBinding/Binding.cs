using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace UiBinding.Core
{
    public abstract class Binding<TSourceIndex, TTargetIndex> : MonoBehaviour
    {
        [SerializeField] private TSourceIndex _sourceIndex;
        [SerializeField] private TTargetIndex _targetIndex;

        [SerializeField] private BindableMonoBehaviour _source;
        [SerializeField] private UnityObject _target;

        private readonly IList<IDisposable> _destructors = new List<IDisposable>();

        protected virtual void Awake()
        {
            AssertNotNull(Source, "Binding source missing.");
            AssertNotNull(Target, "Binding target missing.");
        }

        public BindableMonoBehaviour Source
        {
            get => _source;
            set => _source = value;
        }

        public UnityObject Target
        {
            get => _target;
            set => _target = value;
        }

        public bool HasSource => _source != null;

        public bool HasTarget => _target != null;

        public bool HasSourceAndTarget => HasSource && HasTarget;

        public Type SourceType => _source?.GetType();

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

        public void Break()
        {
            foreach(var destructor in _destructors)
            {
                destructor?.Dispose();
            }

            _destructors.Clear();
        }

        protected void AddDestructor(IDisposable disposable)
        {
            _destructors.Add(disposable);
        }

        protected void AssertNotNull(UnityObject target, string message)
        {
            if(target != null)
            {
                return;
            }

            throw new Exception($"{gameObject.name}: {message}");
        }
    }
}