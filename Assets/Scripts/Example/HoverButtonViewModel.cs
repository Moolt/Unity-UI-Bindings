using UiBinding.Core;
using UnityEngine.EventSystems;

public class HoverButtonViewModel : BindableMonoBehaviour
{
    private bool _hover;

    public bool Hover
    {
        get => _hover;
        set => SetField(ref _hover, value);
    }

    public void OnEventTrigger(EventTriggerType param)
    {
        switch (param)
        {
            case EventTriggerType.PointerEnter: Hover = true; break;
            case EventTriggerType.PointerExit: Hover = false; break;
        }
    }
}
