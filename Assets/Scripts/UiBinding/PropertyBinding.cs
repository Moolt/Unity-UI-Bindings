using System;
using System.ComponentModel;
using System.Reflection;
using UiBinding.Conversion;
using UnityEngine;

namespace UiBinding.Core
{
    public class PropertyBinding : Binding<PropertyIndex, PropertyIndex>
    {
        [SerializeField] private ConverterIndex _converterIndex = new ConverterIndex();
        [SerializeField] private BindingMode _bindingMode;

        private PropertyInfo _sourceProperty;
        private PropertyInfo _targetProperty;
        private IValueConverter _converter;

        private void Awake()
        {
            // Resolve an actual PropertyInfo from the serialized index
            _sourceProperty = SourceIndex.ResolveFrom(Source, PropertyBindingFlags.Source);
            _targetProperty = TargetIndex.ResolveFrom(Target, PropertyBindingFlags.Target);

            // Listen for changes of the source
            if (_bindingMode != BindingMode.OneTime)
            {
                SetupSourceBinding();
            }

            // Listen for changes of the target
            if (_bindingMode == BindingMode.TwoWay)
            {
                UiEventLookup.RegisterIfEventExistsFor(Target, _targetProperty, OnTargetChanged);
            }

            // Resolve the selected converter. 
            // If no converter has been specified, a default converter will be used instead.
            _converter = _converterIndex.ResolveFor(this);

            // Retrieve the initial value from the source.
            InitializeValue();
        }

        public ConverterIndex ConverterIndex => _converterIndex;

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
            OnSourceChanged(this, new PropertyChangedEventArgs(_sourceProperty.Name));
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
        }
    }
}