﻿using System;
using UiBinding.Core;
using UnityEngine;

namespace UiBinding.Conversion
{
    [Serializable]
    public class ConverterIndex
    {
        [SerializeField] private int _index = -1;

        public IValueConverter ResolveFor(PropertyBinding binding)
        {
            if (IsDefault)
            {
                return new DefaultConverter(binding.SourcePropertyType, binding.TargetPropertyType);
            }

            return ConversionProvider.Instantiate(this);
        }

        public static implicit operator int(ConverterIndex index) => index.Index;

        public static implicit operator ConverterIndex(int index) => new ConverterIndex() { Index = index };

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public bool IsDefault => _index < 0;

        public static int Default => -1;
    }
}