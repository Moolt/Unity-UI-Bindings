using System.Linq;
using System.Reflection;
using UiBinding.Core;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
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

        private void OnValidate()
        {
            _binding.UpdateBinding();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space();

            if (_listProperties.ChangeTargetTypeIfNecessary(_binding.SourceType))
            {
                _binding.ListIdentifier.Name = string.Empty;
            }

            _binding.Source = (BindableMonoBehaviour)EditorGUILayout.ObjectField("Source", _binding.Source, typeof(BindableMonoBehaviour), true);

            if (_binding.Source == null)
            {
                return;
            }

            var listIndex = IndexedIdentifier.For(_binding.ListIdentifier, _listProperties.Names);
            listIndex.Index = EditorGUILayout.Popup("List", listIndex.Index, Nicify(_listProperties.Names));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mapping", EditorStyles.boldLabel);

            if (_listProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("Add prefabs which bind to the types in your list.\n", MessageType.Info);
            }

            for (int i = 0; i < _listProperty.arraySize; i++)
            {
                var item = _listProperty.GetArrayElementAtIndex(i);
                var name = TypeNameFromSerializeProperty(item);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name, GUILayout.Width(125));
                EditorGUILayout.PropertyField(item, GUIContent.none);

                if (GUILayout.Button("×", GUILayout.Width(20)))
                {
                    item.objectReferenceValue = null;
                    _listProperty.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add prefab"))
            {
                _listProperty.InsertArrayElementAtIndex(_listProperty.arraySize);
            }

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                MarkDirty();
            }
        }

        private string[] Nicify(string[] input)
        {
            return input.Select(s => ObjectNames.NicifyVariableName(s)).ToArray();
        }

        private void MarkDirty()
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        private string TypeNameFromSerializeProperty(SerializedProperty property)
        {
            string name = "No binding found";
            try
            {
                name = (property.objectReferenceValue as GameObject).GetComponentInChildren<IBinding>().SourceType.Name;
            }
            catch { }

            return name;
        }
    }
}