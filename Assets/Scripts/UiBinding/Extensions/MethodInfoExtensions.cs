using System.Linq;
using System.Reflection;

namespace UiBinding.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool AnyParameters(this MethodInfo method) => method.GetParameters().Any();

        public static bool IsParameterless(this MethodInfo method) => !method.AnyParameters();

        public static bool HasParameter<T>(this MethodInfo method)
        {
            if (!method.AnyParameters())
            {
                return false;
            }

            var parameters = method.GetParameters();

            if (parameters.Count() != 1)
            {
                return false;
            }

            return parameters[0].ParameterType == typeof(T);
        }
    }
}