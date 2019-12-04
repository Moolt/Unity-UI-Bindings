using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UiBinding.Core
{
    public static class UiEventLookup
    {
        private static Dictionary<UiEventLookupKey, string> _lookup = new Dictionary<UiEventLookupKey, string>()
        {
            [(typeof(Slider), "value")] = "onValueChanged",
            [(typeof(InputField), "text")] = "onValueChanged",
            [(typeof(Toggle), "isOn")] = "onValueChanged",
            [(typeof(Scrollbar), "value")] = "onValueChanged",
            [(typeof(Dropdown), "value")] = "onValueChanged",
        };

        public static void RegisterEvent<T>(string eventName, string propertyName)
        {
            RegisterEvent(typeof(T), eventName, propertyName);
        }

        public static void RegisterEvent(Type uiElementType, string eventName, string propertyName)
        {
            _lookup[(uiElementType, propertyName)] = eventName;
        }

        public static void RegisterIfEventExistsFor(UIBehaviour instance, PropertyInfo targetProperty, Action<object> callback)
        {
            if (!_lookup.TryGetValue((targetProperty.DeclaringType, targetProperty.Name), out var eventName))
            {
                return;
            }

            // Unity is inconsistent about how they define Events.
            // They may be properties, as they should be, or public fields, as they absolutely shouldn't be.
            PropertyInfo uiEventProperty = targetProperty.DeclaringType
                .GetProperties(PropertyBindingFlags.Target)
                .Where(p => p.Name == eventName)
                .FirstOrDefault();

            FieldInfo uiEventField = targetProperty.DeclaringType
                .GetFields(PropertyBindingFlags.Target)
                .Where(p => p.Name == eventName)
                .FirstOrDefault();

            if (uiEventProperty == null && uiEventField == null)
            {
                return;
            }

            var unityAction = UnityActionFor(targetProperty.PropertyType, callback);

            if (uiEventProperty != null)
            {
                RegisterAction(instance, uiEventProperty, unityAction);
                return;
            }

            RegisterAction(instance, uiEventField, unityAction);
        }

        public static void RegisterForEvent(UIBehaviour instance, PropertyInfo targetProperty, MethodInfo sourceMethod, Action<object> callback)
        {
            object unityAction;
            var parameters = sourceMethod.GetParameters();

            if (parameters.Length > 1)
            {
                return;
            }

            if (parameters.Length == 1)
            {
                var parameter = parameters.FirstOrDefault();

                if (parameter == null)
                {
                    return;
                }

                var parameterType = parameter.ParameterType;
                unityAction = UnityActionFor(parameterType, callback);
            }
            else
            {
                unityAction = UnityActionFor(callback);
            }

            RegisterAction(instance, targetProperty, unityAction);
        }

        public static bool HasEventFor(Type uiType, string propertyName)
        {
            return _lookup.ContainsKey((uiType, propertyName));
        }

        public static UnityAction<T> GenericUnityActionFromCallback<T>(Action<object> callback)
        {
            return new UnityAction<T>((v) => callback(v));
        }

        public static UnityAction UnityActionFromCallback(Action<object> callback)
        {
            return new UnityAction(() => callback(null));
        }

        private static object UnityActionFor(Type type, Action<object> callback)
        {
            var magicMethod = typeof(UiEventLookup).GetMethod(nameof(UiEventLookup.GenericUnityActionFromCallback));
            var genericMethod = magicMethod.MakeGenericMethod(type);
            return genericMethod.Invoke(null, new object[] { callback });
        }

        private static object UnityActionFor(Action<object> callback)
        {
            return UnityActionFromCallback(callback);
        }

        private static void RegisterAction(UIBehaviour instance, PropertyInfo eventProperty, object unityAction)
        {
            var eventInstance = eventProperty.GetValue(instance);
            var addListener = eventProperty.PropertyType.GetMethod("AddListener");
            addListener.Invoke(eventInstance, new object[] { unityAction });
        }

        private static void RegisterAction(UIBehaviour instance, FieldInfo eventField, object unityAction)
        {
            var eventInstance = eventField.GetValue(instance);
            var addListener = eventField.FieldType.GetMethod("AddListener");
            addListener.Invoke(eventInstance, new object[] { unityAction });
        }
    }
}