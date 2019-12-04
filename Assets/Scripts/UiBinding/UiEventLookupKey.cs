using System;

namespace UiBinding.Core
{
    public class UiEventLookupKey
    {
        public UiEventLookupKey(Type uiElementType, string propertyName)
        {
            UiElementType = uiElementType;
            PropertyName = propertyName;
        }

        public static implicit operator UiEventLookupKey((Type, string) tuple) => new UiEventLookupKey(tuple.Item1, tuple.Item2);

        public Type UiElementType { get; }

        public string PropertyName { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UiEventLookupKey other))
            {
                return false;
            }

            return PropertyName == other.PropertyName && UiElementType == other.UiElementType;
        }

        public override int GetHashCode()
        {
            return PropertyName.GetHashCode() ^ UiElementType.GetHashCode();
        }
    }
}