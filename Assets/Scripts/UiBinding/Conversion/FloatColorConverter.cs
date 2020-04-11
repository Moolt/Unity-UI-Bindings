using UnityEngine;

namespace UiBinding.Conversion
{
    public class FloatColorConverter : ValueConverter<float, Color>
    {
        public override Color Convert(float value)
        {
            return Color.Lerp(MinColor, MaxColor, value);
        }

        public Color MinColor { get; set; } = Color.red;

        public Color MaxColor { get; set; } = Color.blue;
    }
}
