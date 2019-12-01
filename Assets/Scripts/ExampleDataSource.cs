using System;
using System.Windows.Input;
using UnityEngine;

public class Command : ICommand
{
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        Debug.Log("command executed");
    }
}

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

    public void ButtonPressed()
    {
        Debug.Log("Button was pressed!");
    }

    public void Scrolled(float value)
    {
        Debug.Log(value + "scroll!");
    }

    public ICommand ButtonPressedCommand => new Command();
}
