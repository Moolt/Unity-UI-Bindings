using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UiBinding.Core;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiBinding.Inspector
{
    [CustomEditor(typeof(EventTriggerBinding))]
    public class EventTriggerBindingInspector : Editor
    {
        [SerializeField] private EventTriggerBinding _binding;
        [SerializeField] private MemberCollection<MethodInfo> _sourceMethods;
        [SerializeField] private TypeCollection<INotifyPropertyChanged> _sourceTypeCollection;

        private void OnEnable()
        {
            _binding = target as EventTriggerBinding;
            _sourceTypeCollection = new TypeCollection<INotifyPropertyChanged>();
            _sourceMethods = new MemberCollection<MethodInfo>(_binding.SourceType, MemberFilters.SourceCallbacks);
        }

        private void OnValidate()
        {
            if (!IsPrefabStage)
            {
                return;
            }

            //_binding.UpdateBinding();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space();

            if (_sourceMethods.ChangeTargetTypeIfNecessary(_binding.SourceType))
            {
                _binding.SourceIdentifier.Name = string.Empty;
            }

            _binding.SourceDefinition.Kind = (BindingMemberKind)EditorGUILayout.EnumPopup("Reference", _binding.SourceDefinition.Kind);
            _binding.EventTriggerType = (EventTriggerType)EditorGUILayout.EnumPopup("Trigger", _binding.EventTriggerType);


            if (_binding.SourceDefinition.Kind == BindingMemberKind.Instance)
            {
                _binding.SourceDefinition.Instance = (BindableMonoBehaviour)EditorGUILayout.ObjectField("Source", _binding.SourceDefinition.Instance, typeof(BindableMonoBehaviour), true);
            }
            else
            {
                int selected = _sourceTypeCollection.IndexOf(_binding.SourceDefinition?.Type);
                selected = EditorGUILayout.Popup("Source", selected, _sourceTypeCollection.Names.ToArray());
                _binding.SourceDefinition.Type = _sourceTypeCollection.TypeAt(selected);
            }

            if(!_binding.HasSource)
            {
                ResetBinding();
                return;
            }

            var sourceIndex = IndexedIdentifier.For(_binding.SourceIdentifier, _sourceMethods.Names);

            if (_sourceMethods.Any())
            {
                sourceIndex.Index = EditorGUILayout.Popup("Source Callback", sourceIndex.Index, Nicify(_sourceMethods.Names));
            }
            else
            {
                EditorGUILayout.HelpBox("No methods found in source.", MessageType.Info);
                return;
            }
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

        private void MarkDirty()
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        private bool IsPrefabStage => PrefabStageUtility.GetCurrentPrefabStage() != null;

        private string[] Nicify(string[] input)
        {
            return input.Select(s => ObjectNames.NicifyVariableName(s)).ToArray();
        }

        private void ResetBinding()
        {
            _binding.SourceIdentifier.Name = string.Empty;
        }
    }
}