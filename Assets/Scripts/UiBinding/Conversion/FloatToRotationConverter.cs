using UnityEngine;

namespace UiBinding.Conversion
{
    public class FloatToRotationConverter : ValueConverter<float, Quaternion>
    {
        public override Quaternion Convert(float value)
        {
            return Quaternion.Euler(RotationAxis * value * 360);
        }

        public Vector3 RotationAxis { get; set; } = Vector3.back;
    }
}