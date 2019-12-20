using System.Reflection;

namespace UiBinding.Core
{
    public class EventBinding : Binding<MethodIdentifier, PropertyIdentifier>
    {
        private MethodInfo _sourceCallback;
        private PropertyInfo _targetEvent;

        protected override void Awake()
        {
            base.Awake();

            SourceIdentifier.AssertValid($"{gameObject.name}: No source callback found.");
            TargetIdentifier.AssertValid($"{gameObject.name}: No target event found.");

            // Resolve an actual PropertyInfo from the serialized index
            _sourceCallback = SourceIdentifier.ResolveFrom(Source);
            _targetEvent = TargetIdentifier.ResolveFrom(Target);

            var destructor = UiEventLookup.RegisterForEvent(Target, _targetEvent, _sourceCallback, OnTargetChanged);
            AddDestructor(destructor);
        }

        private void OnDestroy()
        {
            Break();
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