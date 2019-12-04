using UnityEngine;

namespace UiBinding.Conversion
{
    public class ColorConverter : ValueConverter<float, Color>
    {
        public override Color Convert(float value)
        {
            return Color.Lerp(Color.red, Color.blue, value);
        }
    }
}
