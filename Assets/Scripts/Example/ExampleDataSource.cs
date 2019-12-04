using UnityEngine;

namespace UiBinding.Core
{
    public class ExampleDataSource : BindableMonoBehaviour
    {
        private float _sliderValue = 0.5f;
        private string _someText;
        private bool _checked = true;

        public float SliderValue
        {
            get => _sliderValue;
            set => SetField(ref _sliderValue, value);
        }

        public string SomeText
        {
            get => _someText;
            set => SetField(ref _someText, value);
        }

        public bool Checked
        {
            get => _checked;
            set => SetField(ref _checked, value);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SliderValue += .05f;
            }
        }

        public void ButtonPressed()
        {
            Debug.Log("Button was pressed!");
        }

        public void Scrolled(float value)
        {
            Debug.Log(value + "scroll!");
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            Debug.Log(propertyName);
        }
    }
}