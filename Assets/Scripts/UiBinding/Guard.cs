using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace UiBinding
{
    public static class Guard
    {
        public static void AssertNotNull(GameObject caller, object target, string message)
        {
            if (target is UnityObject unityObject && unityObject != null)
            {
                return;
            }

            if (target != null)
            {
                return;
            }

            throw new Exception($"{caller.name}: {message}");
        }

    }
}
