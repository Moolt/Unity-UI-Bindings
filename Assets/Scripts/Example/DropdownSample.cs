using System.Collections.Generic;
using System.Collections.ObjectModel;
using UiBinding.Core;

public class DropdownSample : BindableMonoBehaviour
{
    private int _selectedIndex;
    private string _newItemName;

    private ObservableCollection<string> _options = new ObservableCollection<string>()
    {
        "Apple",
        "Pear",
        "Banana",
    };

    public ObservableCollection<string> Options
    {
        get => _options;
        set => SetField(ref _options, value);
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            SetField(ref _selectedIndex, value);
            RaisePropertyChanged(() => SelectedItem);
        }
    }

    public string SelectedItem => _options[_selectedIndex];

    public string NewItemName
    {
        get => _newItemName;
        set => SetField(ref _newItemName, value);
    }

    public void OnUserAddedItem()
    {
        Options.Add(NewItemName);
        NewItemName = string.Empty;
        SelectedIndex = Options.Count - 1;
    }
}
