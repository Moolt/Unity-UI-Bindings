using System.Collections.Generic;
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

        _binding.Source = (BindableMonoBehaviour)EditorGUILayout.ObjectField("Source", _binding.Source, _binding.SourceType, true);
        _binding.Target = (UIBehaviour)EditorGUILayout.ObjectField("Target", _binding.Target, _binding.TargetType, true);


        if (!_binding.HasSourceAndTarget)
        {
            return;
        }

        GuiLine();

        _binding.SourceIndex = EditorGUILayout.Popup("Source Property", _binding.SourceIndex, _sourceProperties.PropertyNames);
        _binding.TargetIndex = EditorGUILayout.Popup("Target Property", _binding.TargetIndex, _targetProperties.PropertyNames);

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
}
