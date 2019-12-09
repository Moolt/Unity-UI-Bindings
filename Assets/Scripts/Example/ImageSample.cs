using UiBinding.Core;
using UnityEngine;

public class ImageSample : BindableMonoBehaviour
{
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private Sprite _onSprite;

    private bool _checked;
    private string _text = string.Empty;

    public bool Checked
    {
        get => _checked;
        set
        {
            SetField(ref _checked, value);
            RaisePropertyChanged(() => Sprite);
        }
    }

    public Sprite Sprite => _checked ? _onSprite : _offSprite;
}
