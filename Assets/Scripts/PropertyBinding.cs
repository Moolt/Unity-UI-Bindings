using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropertyBinding : MonoBehaviour
{
    [SerializeField] private BindableMonoBehaviour _source;
    [SerializeField] private UIBehaviour _target;

    [HideInInspector] [SerializeField] private PropertyIndex _sourceIndex;
    [HideInInspector] [SerializeField] private PropertyIndex _targetIndex;
    [HideInInspector] [SerializeField] private ConverterIndex _converterIndex;

    private PropertyInfo _sourceProperty;
    private PropertyInfo _targetProperty;
    private IValueConverter _converter;

    private void Awake()
    {
        // Resolve an actual PropertyInfo from the serialized index
        _sourceProperty = _sourceIndex.ResolveFrom(_source);
        _targetProperty = _targetIndex.ResolveFrom(_target);

        // Resolve the selected converter. 
        // If no converter has been specified, a default converter will be used instead.
        _converter = _converterIndex.ResolveFor(this);

        // Listen for changes of the source
        _source.PropertyChanged += OnSourceChanged;

        // Listen for changes of the target
        UiEventLookup.RegisterIfEventExistsFor(_target, _targetProperty, OnTargetChanged);

        // Retrieve the initial value from the source.
        InitializeValue();
    }

    public BindableMonoBehaviour Source
    {
        get => _source;
        set => _source = value;
    }

    public UIBehaviour Target
    {
        get => _target;
        set => _target = value;
    }

    public int SourceIndex
    {
        get => _sourceIndex;
        set => _sourceIndex = value;
    }

    public int TargetIndex
    {
        get => _targetIndex;
        set => _targetIndex = value;
    }

    public int ConverterIndex
    {
        get => _converterIndex;
        set => _converterIndex = value;
    }

    public Type SourceType => _source?.GetType();

    public Type TargetType => _target?.GetType();

    public Type SourcePropertyType => _sourceProperty?.PropertyType;

    public Type TargetPropertyType => _targetProperty?.PropertyType;

    public bool HasSource => _source != null;

    public bool HasTarget => _target != null;

    public bool HasSourceAndTarget => HasSource && HasTarget;

    public bool HasMatchingTypes => TargetPropertyType != null && SourcePropertyType != null && TargetPropertyType == SourcePropertyType;

    private void OnSourceChanged(object sender, PropertyChangedEventArgs e)
    {
        var sourceValue = _sourceProperty.GetValue(_source);
        var converted = _converter.Convert(sourceValue);
        _targetProperty.SetValue(_target, converted);
    }

    private void OnTargetChanged(object value)
    {
        var converted = _converter.ConvertBack(value);
        _sourceProperty.SetValue(_source, converted);
    }

    private void InitializeValue()
    {
        OnSourceChanged(this, new PropertyChangedEventArgs(_sourceProperty.Name));
    }
}
