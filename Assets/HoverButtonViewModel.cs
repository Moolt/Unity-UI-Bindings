using UiBinding.Core;

public class HoverButtonViewModel : BindableMonoBehaviour
{
    private bool _hover;

    public bool Hover
    {
        get => _hover;
        set => SetField(ref _hover, value);
    }

    public void Entered()
    {
        Hover = true;
    }

    public void Left()
    {
        Hover = false;
    }
}
