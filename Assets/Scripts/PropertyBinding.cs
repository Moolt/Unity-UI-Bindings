using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class PropertyBinding : Binding<PropertyIndex, PropertyIndex>
{
    [SerializeField] private ConverterIndex _converterIndex;
    [SerializeField] private BindingMode _bindingMode;

    private PropertyInfo _sourceProperty;
    private PropertyInfo _targetProperty;
    private IValueConverter _converter;

    private void Awake()
    {
        // Resolve an actual PropertyInfo from the serialized index
        _sourceProperty = SourceIndex.ResolveFrom(Source, PropertyBindingFlags.Source);
        _targetProperty = TargetIndex.ResolveFrom(Target, PropertyBindingFlags.Target);

        // Resolve the selected converter. 
        // If no converter has been specified, a default converter will be used instead.
        _converter = _converterIndex.ResolveFor(this);

        // Listen for changes of the source
        if (_bindingMode != BindingMode.OneTime)
        {
            Source.PropertyChanged += OnSourceChanged;
        }

        // Listen for changes of the target
        if (_bindingMode == BindingMode.TwoWay)
        {
            UiEventLookup.RegisterIfEventExistsFor(Target, _targetProperty, OnTargetChanged);
        }

        // Retrieve the initial value from the source.
        InitializeValue();
    }

    public int ConverterIndex
    {
        get => _converterIndex;
        set => _converterIndex = value;
    }

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
}
