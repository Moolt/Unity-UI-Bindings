﻿using System.Linq;
using System.Reflection;
using UiBinding.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiBinding.Inspector
{
    [CustomEditor(typeof(EventBinding))]
    public class EventBindingEditor : Editor
    {
        [SerializeField] private EventBinding _binding;
        [SerializeField] private MemberCollection<MethodInfo> _sourceCallbacks;
        [SerializeField] private MemberCollection<PropertyInfo> _targetEvents;

        private void OnEnable()
        {
            _binding = target as EventBinding;

            _sourceCallbacks = new MemberCollection<MethodInfo>(_binding.SourceType, MemberFilters.SourceCallbacks);
            _targetEvents = new MemberCollection<PropertyInfo>(_binding.TargetType, MemberFilters.TargetEvents);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space();

            _targetEvents.ChangeTargetTypeIfNecessary(_binding.TargetType);
            _sourceCallbacks.ChangeTargetTypeIfNecessary(_binding.SourceType);

            _binding.Source = (BindableMonoBehaviour)EditorGUILayout.ObjectField("Source", _binding.Source, typeof(BindableMonoBehaviour), true);
            _binding.Target = (UIBehaviour)EditorGUILayout.ObjectField("Target", _binding.Target, typeof(UIBehaviour), true);

            if (!_binding.HasSourceAndTarget)
            {
                ResetBinding();
                return;
            }

            GuiLine();

            _binding.SourceIndex = EditorGUILayout.Popup("Callback", _binding.SourceIndex, Nicify(_sourceCallbacks.Names));
            _binding.TargetIndex = EditorGUILayout.Popup("Event", _binding.TargetIndex, Nicify(_targetEvents.Names));

            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        private void ResetBinding()
        {
            _binding.SourceIndex = 0;
            _binding.TargetIndex = 0;
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

        private string[] Nicify(string[] input)
        {
            return input.Select(s => ObjectNames.NicifyVariableName(s)).ToArray();
        }
    }
}