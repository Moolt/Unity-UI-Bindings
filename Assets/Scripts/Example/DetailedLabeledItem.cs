using UiBinding.Core;

public class DetailedLabeledItem : LabeledItem
{
    private string _details;

    public DetailedLabeledItem(string text, string details) : base(text)
    {
        _details = details;
    }

    public string Details
    {
        get => _details;
        set => SetField(ref _details, value);
    }
}
