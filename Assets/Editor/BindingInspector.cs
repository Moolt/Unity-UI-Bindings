using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropertyBinding))]
public class BindingInspector : Editor
{
    [SerializeField] private PropertyBinding _binding;
    [SerializeField] private PropertiesCollection _sourceProperties;
    [SerializeField] private PropertiesCollection _targetProperties;

    private void OnEnable()
    {
        _binding = target as PropertyBinding;

        _sourceProperties = PropertiesCollection.For(_binding.SourceType);
        _targetProperties = PropertiesCollection.For(_binding.TargetType);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _sourceProperties.ChangeTargetTypeIfNecessary(_binding.SourceType);
        _targetProperties.ChangeTargetTypeIfNecessary(_binding.TargetType);

        if (!_binding.HasSourceAndTarget)
        {
            return;
        }

        _binding.SourceIndex = EditorGUILayout.Popup(" ", _binding.SourceIndex, _sourceProperties.PropertyNames);
        _binding.TargetIndex = EditorGUILayout.Popup(" ", _binding.TargetIndex, _targetProperties.PropertyNames);

        if (!_binding.HasMatchingTypes)
        {
            EditorGUILayout.HelpBox("Source type differs from target type.\nConsider using a value converter.", MessageType.Warning);
        }
    }
}
