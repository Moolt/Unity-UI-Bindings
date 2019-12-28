using System.Linq;
using System.Reflection;
using UiBinding.Core;
using UnityEditor;
using UnityEngine;

namespace UiBinding.Inspector
{
    [CustomEditor(typeof(ListBinding))]
    public class ListBindingInspector : Editor
    {
        [SerializeField] private ListBinding _binding;
        [SerializeField] private MemberCollection<PropertyInfo> _listProperties;
        [SerializeField] private SerializedProperty _listProperty;

        private void OnEnable()
        {
            _binding = target as ListBinding;
            _listProperties = new MemberCollection<PropertyInfo>(_binding.SourceType, MemberFilters.Lists);
            _listProperty = serializedObject.FindProperty("_prefabs");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            if (_listProperties.ChangeTargetTypeIfNecessary(_binding.SourceType))
            {
                _binding.ListIdentifier.Name = string.Empty;
            }

            _binding.Source = (BindableMonoBehaviour)EditorGUILayout.ObjectField("Source", _binding.Source, typeof(BindableMonoBehaviour), true);

            if(_binding.Source == null)
            {
                return;
            }

            var listIndex = IndexedIdentifier.For(_binding.ListIdentifier, _listProperties.Names);
            listIndex.Index = EditorGUILayout.Popup("List", listIndex.Index, Nicify(_listProperties.Names));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_listProperty, true);
            serializedObject.ApplyModifiedProperties();
        }

        private string[] Nicify(string[] input)
        {
            return input.Select(s => ObjectNames.NicifyVariableName(s)).ToArray();
        }
    }
}