using UnityEngine;

namespace UiBinding.Conversion
{
    public class BoolColorConverter : ValueConverter<bool, Color>
    {
        public override Color Convert(bool value)
        {
            return value ? TrueColor : FalseColor;
        }

        public Color FalseColor { get; set; } = Color.red;

        public Color TrueColor { get; set; } = Color.green;
    }
}
