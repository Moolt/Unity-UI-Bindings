using UiBinding.Core;

public class CheckboxSample : BindableMonoBehaviour
{
    private bool _checked;
    private string _text = string.Empty;

    public bool Checked
    {
        get => _checked;
        set => SetField(ref _checked, value);
    }

    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }
}
