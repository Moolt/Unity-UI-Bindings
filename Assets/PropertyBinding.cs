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

    private PropertyInfo _sourceProperty;
    private PropertyInfo _targetProperty;

    private void Awake()
    {
        _sourceProperty = _sourceIndex.ResolveFrom(_source);
        _targetProperty = _targetIndex.ResolveFrom(_target);
        _source.PropertyChanged += OnSourceChanged;
        UiEventLookup.RegisterIfEventExistsFor(_target, _targetProperty, OnTargetChanged);
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

        if (TargetPropertyType != SourcePropertyType)
        {
            var convertedValue = Convert.ChangeType(sourceValue, TargetPropertyType);
            _targetProperty.SetValue(_target, convertedValue);
            return;
        }

        _targetProperty.SetValue(_target, sourceValue);
    }

    private void OnTargetChanged(object value)
    {
        Debug.Log(value);
        _sourceProperty.SetValue(_source, value);
    }
}
