using System.Collections.Generic;
using System.Linq;
using UiBinding.Core;

public class ListBindingExample : BindableMonoBehaviour
{
    private List<LabeledItem> _items = new List<LabeledItem>();
    private string _searchQuery = string.Empty;

    public List<LabeledItem> Items => _items.Where(i => i.Text.ToLower().Contains(SearchQuery.ToLower())).ToList();

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            SetField(ref _searchQuery, value);
            RaisePropertyChanged(() => Items);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _items.Add(new LabeledItem("Apple"));
        _items.Add(new LabeledItem("Pear"));
        _items.Add(new LabeledItem("Bread"));
        _items.Add(new LabeledItem("Pie"));
        _items.Add(new LabeledItem("Flower"));
        _items.Add(new LabeledItem("Vase"));
        _items.Add(new LabeledItem("House"));
        RaisePropertyChanged(() => Items);
    }
}
