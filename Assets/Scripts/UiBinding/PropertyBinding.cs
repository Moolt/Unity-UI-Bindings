using System;
using System.ComponentModel;
using System.Reflection;
using UiBinding.Conversion;
using UnityEngine;

namespace UiBinding.Core
{
    public class PropertyBinding : Binding<PropertyIdentifier, PropertyIdentifier>
    {
        [SerializeField] private ConverterIdentifier _converterIndex = new ConverterIdentifier();
        [SerializeField] private BindingMode _bindingMode;

        private PropertyInfo _sourceProperty;
        private PropertyInfo _targetProperty;
        private IValueConverter _converter;

        protected override void Awake()
        {
            base.Awake();

            // Resolve an actual PropertyInfo from the serialized index
            _sourceProperty = SourceIdentifier.ResolveFrom(Source);
            _targetProperty = TargetIdentifier.ResolveFrom(Target);

            // Listen for changes of the source
            if (_bindingMode != BindingMode.OneTime)
            {
                SetupSourceBinding();
            }

            // Listen for changes of the target
            if (_bindingMode == BindingMode.TwoWay)
            {
                var destructor = UiEventLookup.RegisterIfEventExistsFor(Target, _targetProperty, OnTargetChanged);
                AddDestructor(destructor);
            }

            // Resolve the selected converter. 
            // If no converter has been specified, a default converter will be used instead.
            _converter = _converterIndex.ResolveFor(this);

            // Retrieve the initial value from the source.
            InitializeValue();
        }

        private void OnDestroy()
        {
            Break();
        }

        public ConverterIdentifier ConverterIdentifier => _converterIndex;

        public BindingMode BindingMode
        {
            get => _bindingMode;
            set => _bindingMode = value;
        }

        public Type SourcePropertyType => _sourceProperty?.PropertyType;

        public Type TargetPropertyType => _targetProperty?.PropertyType;

        public bool HasMatchingTypes => TargetPropertyType != null && SourcePropertyType != null && TargetPropertyType == SourcePropertyType;

        private void OnSourceChanged(object sender, PropertyChangedEventArgs e)
        {
            var sourceValue = _sourceProperty.GetValue(Source);
            var converted = _converter.Convert(sourceValue);
            _targetProperty.SetValue(Target, converted);
        }

        private void OnTargetChanged(object value)
        {
            var converted = _converter.ConvertBack(value);
            _sourceProperty.SetValue(Source, converted);
        }

        private void InitializeValue()
        {
            try
            {
                OnSourceChanged(this, new PropertyChangedEventArgs(_sourceProperty.Name));
            }
            catch (Exception e)
            {
                Break();
                Debug.LogError($"A binding broke because of an error. Please fix all errors, as other bindings might also be affected.\n{ToString()}");
                Debug.LogException(e);
            }
        }

        private void SetupSourceBinding()
        {
            INotifyPropertyChanged bindingSource = Source;

            // Enables support for types like observable collection.
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(_sourceProperty.PropertyType))
            {
                bindingSource = (INotifyPropertyChanged)_sourceProperty.GetValue(Source);
            }

            bindingSource.PropertyChanged += OnSourceChanged;

            // Unregister the subscription if the binding breaks.
            var destructor = new DisposableAction(() => bindingSource.PropertyChanged -= OnSourceChanged);
            AddDestructor(destructor);
        }

        public override string ToString()
        {
            return $"{gameObject.name}: {SourceType.Name}::{_sourceProperty.Name} -> {TargetType.Name}::{_targetProperty.Name}";
        }
    }
}