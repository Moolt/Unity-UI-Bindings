using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UiEventLookup
{
    private static Dictionary<UiEventLookupKey, string> _lookup = new Dictionary<UiEventLookupKey, string>()
    {
        [(typeof(Slider), "value")] = "onValueChanged",
        [(typeof(InputField), "text")] = "onValueChanged",
        [(typeof(Toggle), "isOn")] = "onValueChanged",
    };

    public static void RegisterEvent<T>(string eventName, string propertyName)
    {
        RegisterEvent(typeof(T), eventName, propertyName);
    }

    public static void RegisterEvent(Type uiElementType, string eventName, string propertyName)
    {
        _lookup[(uiElementType, propertyName)] = eventName;
    }

    public static void RegisterIfEventExistsFor<T>(PropertyInfo property, Action<object> callback)
    {
        //RegisterIfEventExistsFor(typeof(T), property, callback);
    }

    public static void RegisterIfEventExistsFor(UIBehaviour instance, PropertyInfo property, Action<object> callback)
    {
        if (!_lookup.TryGetValue((property.DeclaringType, property.Name), out var eventName))
        {
            return;
        }

        var uiEvent = property.DeclaringType
            .GetProperties(PropertyBindingFlags.Default)
            .Where(p => p.Name == eventName)
            .FirstOrDefault();

        if (uiEvent == null)
        {
            return;
        }

        var magicMethod = typeof(UiEventLookup).GetMethod("UnityActionFromCallback");
        var genericMethod = magicMethod.MakeGenericMethod(property.PropertyType);
        var unityAction = genericMethod.Invoke(null, new object[] { callback });

        var eventInstance = uiEvent.GetValue(instance);
        var addListener = uiEvent.PropertyType.GetMethod("AddListener");
        addListener.Invoke(eventInstance, new object[] { unityAction });
    }

    public static UnityAction<T> UnityActionFromCallback<T>(Action<object> callback)
    {
        return new UnityAction<T>((v) => callback(v));
    }

    public static bool HasEventFor(Type uiType, string propertyName)
    {
        return _lookup.ContainsKey((uiType, propertyName));
    }
}