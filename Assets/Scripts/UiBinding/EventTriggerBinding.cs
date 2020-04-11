using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

namespace UiBinding.Core
{
    public class EventTriggerBinding : MonoBehaviour
    {
        private EventTrigger _target;
        private Entry _entry;

        [SerializeField] private MethodIdentifier _sourceIdentifier;
        [SerializeField] private BindingMemberDefinition _sourceDefinition = new BindingMemberDefinition();
        [SerializeField] private EventTriggerType _eventTriggerType;
        [SerializeField] private INotifyPropertyChanged _source;

        private void Awake()
        {
            _target = GetComponentInChildren<EventTrigger>();
            Guard.AssertNotNull(gameObject, _target, "No EventTrigger has been found.");

            OnEstablishBinding();
        }

        public BindingMemberDefinition SourceDefinition
        {
            get => _sourceDefinition;
            private set => _sourceDefinition = value;
        }

        public INotifyPropertyChanged Source
        {
            get => _sourceDefinition.Kind == BindingMemberKind.Instance ? _sourceDefinition.Instance : _source;
            set
            {
                if (_sourceDefinition.Kind == BindingMemberKind.Instance)
                {
                    _sourceDefinition.Instance = value as BindableMonoBehaviour;
                    return;
                }

                _source = value;
            }
        }

        public Type SourceType
        {
            get
            {
                switch (_sourceDefinition.Kind)
                {
                    case BindingMemberKind.Type: return _sourceDefinition.Type;
                    default: return _sourceDefinition.Instance?.GetType();
                }
            }
        }

        public MethodIdentifier SourceIdentifier
        {
            get => _sourceIdentifier;
            set => _sourceIdentifier = value;
        }

        public EventTriggerType EventTriggerType
        {
            get => _eventTriggerType;
            set => _eventTriggerType = value;
        }

        public void Break()
        {
            if (_entry == null)
            {
                return;
            }

            _target.triggers.Remove(_entry);
            _entry = null;
        }

        protected void OnEstablishBinding()
        {
            var methodInfo = SourceIdentifier.ResolveFrom(Source);
            Action<BaseEventData> callback = b => methodInfo.Invoke(Source, new object[] { });
            Subscribe(callback);
        }

        private void Subscribe(Action<BaseEventData> callback)
        {
            var action = new UnityAction<BaseEventData>(callback);
            var triggerEvent = new TriggerEvent();
            triggerEvent.AddListener(action);
            _entry = new Entry() { eventID = EventTriggerType, callback = triggerEvent };
            _target.triggers.Add(_entry);
        }
    }
}
