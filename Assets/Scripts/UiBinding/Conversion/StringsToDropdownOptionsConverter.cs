using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace UiBinding.Conversion
{
    public class StringsToDropdownOptionsConverter : ValueConverter<IEnumerable<string>, List<Dropdown.OptionData>>
    {
        public override List<Dropdown.OptionData> Convert(IEnumerable<string> value)
        {
            return value.Select(s => new Dropdown.OptionData(s)).ToList();
        }
    }
}