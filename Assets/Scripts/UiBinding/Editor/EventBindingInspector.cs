﻿using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UiBinding.Core;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace UiBinding.Inspector
{
    [CustomEditor(typeof(EventBinding))]
    public class EventBindingEditor : Editor
    {
        [SerializeField] private EventBinding _binding;
        [SerializeField] private MemberCollection<MethodInfo> _sourceCallbacks;
        [SerializeField] private MemberCollection<PropertyInfo> _targetEvents;
        [SerializeField] private TypeCollection<INotifyPropertyChanged> _sourceTypeCollection;

        private void OnEnable()
        {
            _binding = target as EventBinding;

            _sourceTypeCollection = new TypeCollection<INotifyPropertyChanged>();
            _sourceCallbacks = new MemberCollection<MethodInfo>(_binding.SourceType, MemberFilters.EventCallbacks);
            _targetEvents = new MemberCollection<PropertyInfo>(_binding.TargetType, MemberFilters.TargetEvents);
        }

        private void OnValidate()
        {
            if (!IsPrefabStage)
            {
                return;
            }

            _binding.UpdateBinding();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space();

            _targetEvents.ChangeTargetTypeIfNecessary(_binding.TargetType);
            _sourceCallbacks.ChangeTargetTypeIfNecessary(_binding.SourceType);

            _binding.SourceDefinition.Kind = (BindingMemberKind)EditorGUILayout.EnumPopup("Reference", _binding.SourceDefinition.Kind);

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

            _binding.Target = EditorGUILayout.ObjectField("Target", _binding.Target, typeof(UnityObject), true);

            if (!_binding.HasSourceAndTarget)
            {
                ResetBinding();
                return;
            }

            GuiLine();

            if (_sourceCallbacks.Count == 0)
            {
                EditorGUILayout.HelpBox("No source callbacks found.", MessageType.Info);
                _binding.SourceIdentifier.Valid = false;
                return;
            }

            if (_targetEvents.Count == 0)
            {
                EditorGUILayout.HelpBox("No target events found.", MessageType.Info);
                _binding.TargetIdentifier.Valid = false;
                return;
            }

            var sourceIndex = IndexedIdentifier.For(_binding.SourceIdentifier, _sourceCallbacks.Names);
            var targetIndex = IndexedIdentifier.For(_binding.TargetIdentifier, _targetEvents.Names);

            sourceIndex.Index = EditorGUILayout.Popup("Callback", sourceIndex.Index, Nicify(_sourceCallbacks.Names));
            targetIndex.Index = EditorGUILayout.Popup("Event", targetIndex.Index, Nicify(_targetEvents.Names));

            if (EditorGUI.EndChangeCheck())
            {
                MarkDirty();
            }
        }

        private void ResetBinding()
        {
            _binding.SourceIdentifier.Name = string.Empty;
            _binding.TargetIdentifier.Name = string.Empty;
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
    }
}