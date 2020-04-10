using UnityEngine;

public class ClickableItem : LabeledItem
{
    public ClickableItem(string text) : base(text)
    {
    }

    public void OnClick()
    {
        Debug.Log(Text);
    }
}
