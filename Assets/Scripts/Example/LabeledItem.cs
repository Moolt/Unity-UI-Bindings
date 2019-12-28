using UiBinding.Core;

public class LabeledItem : BindableObject
{
    private string _text;

    public LabeledItem(string text)
    {
        _text = text;
    }

    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }
}
