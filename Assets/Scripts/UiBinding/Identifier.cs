using System;
using UnityEngine;

namespace UiBinding.Core
{
    [Serializable]
    public abstract class Identifier
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _valid = true;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _valid = true;
            }
        }

        public bool Valid
        {
            get => _valid;
            set => _valid = value;
        }

        public void AssertValid(string message)
        {
            if (_valid)
            {
                return;
            }

            throw new Exception(message);
        }
    }
}