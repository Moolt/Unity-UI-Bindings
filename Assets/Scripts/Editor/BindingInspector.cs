﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[CustomEditor(typeof(PropertyBinding))]
public class BindingInspector : Editor
{
    [SerializeField] private PropertyBinding _binding;
    [SerializeField] private PropertiesCollection _sourceProperties;
    [SerializeField] private PropertiesCollection _targetProperties;
    [SerializeField] private string[] _converters;
    [SerializeField] private string[] _bindingModes = Enum.GetNames(typeof(BindingMode)).ToArray();

    private void OnEnable()
    {
        _binding = target as PropertyBinding;

        _sourceProperties = PropertiesCollection.For(_binding.SourceType);
        _targetProperties = PropertiesCollection.For(_binding.TargetType);

        _converters = AssembleConverterList();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        _targetProperties.ChangeTargetTypeIfNecessary(_binding.TargetType);
        _sourceProperties.ChangeTargetTypeIfNecessary(_binding.SourceType);

        _binding.Source = (BindableMonoBehaviour)EditorGUILayout.ObjectField("Source", _binding.Source, typeof(BindableMonoBehaviour), true);
        _binding.Target = (UIBehaviour)EditorGUILayout.ObjectField("Target", _binding.Target, typeof(UIBehaviour), true);

        if (!_binding.HasSourceAndTarget)
        {
            ResetBinding();
            return;
        }

        GuiLine();

        _binding.SourceIndex = EditorGUILayout.Popup("Source Property", _binding.SourceIndex, _sourceProperties.PropertyNames);
        _binding.TargetIndex = EditorGUILayout.Popup("Target Property", _binding.TargetIndex, _targetProperties.PropertyNames);
        _binding.BindingMode = (BindingMode)EditorGUILayout.Popup("Mode", (int)_binding.BindingMode, _bindingModes);

        if (!TwoWayAvailable && _binding.BindingMode == BindingMode.TwoWay)
        {
            EditorGUILayout.HelpBox("Not available. Will default to OneWay", MessageType.Info);
        }

        GuiLine();

        _binding.ConverterIndex = EditorGUILayout.Popup("Conversion", _binding.ConverterIndex + 1, _converters) - 1;
    }

    private string[] AssembleConverterList()
    {
        var converters = new List<string>() { "Default" };
        converters.AddRange(ConversionProvider.AvailableConverterNames);
        return converters.ToArray();
    }

    /// <summary>
    /// See: https://forum.unity.com/threads/horizontal-line-in-editor-window.520812/
    /// Also: How does Unity still doesn't have this implemented?
    /// </summary>
    private void GuiLine()
    {
        EditorGUILayout.Space();
        var rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();
    }

    private void ResetBinding()
    {
        _binding.BindingMode = default;
        _binding.SourceIndex = 0;
        _binding.TargetIndex = 0;
        _binding.ConverterIndex = ConverterIndex.Default;
    }

    private bool TwoWayAvailable =>
        _binding != null &&
        _binding.HasSourceAndTarget &&
        UiEventLookup.HasEventFor(_binding.TargetType, _targetProperties.PropertyNames[_binding.TargetIndex]);
}
