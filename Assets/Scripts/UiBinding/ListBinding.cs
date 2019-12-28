using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace UiBinding.Core
{
    public class ListBinding : MonoBehaviour
    {
        [SerializeField] private BindableMonoBehaviour _source;
        [SerializeField] private PropertyIdentifier _listIdentifier;
        [SerializeField] private GameObject[] _prefabs;

        private PropertyInfo _listProperty;
        private Dictionary<Type, GameObject> _mapping = new Dictionary<Type, GameObject>();

        private void Awake()
        {
            _listProperty = ListIdentifier.ResolveFrom(Source);
            SetupSourceBinding();
            SetupMapping();
        }

        public BindableMonoBehaviour Source
        {
            get => _source;
            set => _source = value;
        }

        public Type SourceType => Source?.GetType();

        public PropertyIdentifier ListIdentifier
        {
            get => _listIdentifier;
            set => _listIdentifier = value;
        }

        public GameObject[] Prefabs
        {
            get => _prefabs;
            set => _prefabs = value;
        }

        private void SetupSourceBinding()
        {
            INotifyPropertyChanged bindingSource = Source;

            // Enables support for types like observable collection.
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(_listProperty.PropertyType))
            {
                bindingSource = (INotifyPropertyChanged)_listProperty.GetValue(Source);
            }

            bindingSource.PropertyChanged += OnSourceChanged;
        }

        private void SetupMapping()
        {
            foreach (var prefab in _prefabs)
            {
                var binding = prefab.GetComponentInChildren<IBinding>();

                if (binding == null)
                {
                    continue;
                }

                _mapping[binding.SourceType] = prefab;
            }
        }

        private void OnSourceChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var list = _listProperty.GetValue(Source) as IEnumerable;

            foreach (INotifyPropertyChanged item in list)
            {
                if (!_mapping.ContainsKey(item.GetType()))
                {
                    throw new Exception($"No prefab found for type {item.GetType().Name}.");
                }

                var prefab = _mapping[item.GetType()];
                var instance = Instantiate(prefab, transform);
                var bindings = instance.GetComponentsInChildren<IBinding>();

                foreach (var binding in bindings)
                {
                    binding.Bind(item);
                }
            }
        }
    }
}