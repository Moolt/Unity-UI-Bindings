using System.Collections.Generic;
using UiBinding.Core;

public class ClickableItemsListExample : BindableMonoBehaviour
{
    private List<ClickableItem> _items = new List<ClickableItem>();

    public List<ClickableItem> Items => _items;

    void Start()
    {
        _items.Add(new ClickableItem("Try"));
        _items.Add(new ClickableItem("to"));
        _items.Add(new ClickableItem("click"));
        _items.Add(new ClickableItem("an"));
        _items.Add(new ClickableItem("item"));
        _items.Add(new ClickableItem(":)"));
        RaisePropertyChanged(() => Items);
    }
}
