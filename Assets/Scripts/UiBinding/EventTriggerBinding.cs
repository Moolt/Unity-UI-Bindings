using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using UnityObject = UnityEngine.Object;

namespace UiBinding.Core
{
    public class EventTriggerBinding : Binding<MethodIdentifier, EventTrigger>
    {
        private Entry _entry;

        [SerializeField] private EventTriggerType _eventTriggerType;

        public EventTriggerType EventTriggerType
        {
            get => _eventTriggerType;
            set => _eventTriggerType = value;
        }

        public override UnityObject Target
        {
            get => TargetIdentifier;
            set => TargetIdentifier = value as EventTrigger;
        }

        protected override void BeforeAwake()
        {
            base.BeforeAwake();
            Target = GetComponentInChildren<EventTrigger>();
            Guard.AssertNotNull(gameObject, Target, "No EventTrigger has been found.");
        }

        protected override void OnEstablishBinding()
        {
            var methodInfo = SourceIdentifier.ResolveFrom(Source);
            Action<BaseEventData> callback = b => methodInfo.Invoke(Source, new object[] { });
            Subscribe(callback);
            AddDestructor(new DisposableAction(RemoveSubscription));
        }

        private void Subscribe(Action<BaseEventData> callback)
        {
            var action = new UnityAction<BaseEventData>(callback);
            var triggerEvent = new TriggerEvent();
            triggerEvent.AddListener(action);
            _entry = new Entry() { eventID = EventTriggerType, callback = triggerEvent };
            TargetIdentifier.triggers.Add(_entry);
        }

        private void RemoveSubscription()
        {
            if (_entry == null)
            {
                return;
            }

            TargetIdentifier.triggers.Remove(_entry);
            _entry = null;
        }
    }
}
