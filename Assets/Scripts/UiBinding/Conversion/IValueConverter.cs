namespace UiBinding.Conversion
{
    public interface IValueConverter
    {
        object Convert(object value);

        object ConvertBack(object value);
    }
}