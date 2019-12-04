using UiBinding.Core;
using UnityEngine;

public class TimerDataSource : BindableMonoBehaviour
{
    private float _timerValue;

    public float TimerValue
    {
        get => _timerValue;
        set => SetField(ref _timerValue, value);
    }

    private void Update()
    {
        TimerValue = Time.time;
    }
}
