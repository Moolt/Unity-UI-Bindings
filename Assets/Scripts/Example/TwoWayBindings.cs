using UiBinding.Core;

public class TwoWayBindings : BindableMonoBehaviour
{
    private float _sliderValue = 0.5f;

    public float SliderValue
    {
        get => _sliderValue;
        set => SetField(ref _sliderValue, value);
    }
}
