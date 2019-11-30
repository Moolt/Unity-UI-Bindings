using UnityEngine;

public class ExampleDataSource : BindableMonoBehaviour
{
    private float _sliderValue = 0.5f;
    private string _someText;

    public float SliderValue
    {
        get => _sliderValue;
        set => SetField(ref _sliderValue, value);
    }
    public string SomeText
    {
        get => _someText;
        set => SetField(ref _someText, value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SliderValue += .05f;
        }
    }
}
