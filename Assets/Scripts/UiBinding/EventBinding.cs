using System.Reflection;

namespace UiBinding.Core
{
    public class EventBinding : Binding<MethodIndex, PropertyIndex>
    {
        private MethodInfo _sourceCallback;
        private PropertyInfo _targetEvent;

        private void Awake()
        {
            // Resolve an actual PropertyInfo from the serialized index
            _sourceCallback = SourceIndex.ResolveFrom(Source, PropertyBindingFlags.Source);
            _targetEvent = TargetIndex.ResolveFrom(Target, PropertyBindingFlags.Target);

            UiEventLookup.RegisterForEvent(Target, _targetEvent, _sourceCallback, OnTargetChanged);
        }

        private void OnTargetChanged(object value)
        {
            if (value == null)
            {
                _sourceCallback.Invoke(Source, new object[] { });
                return;
            }

            _sourceCallback.Invoke(Source, new object[] { value });
        }
    }
}