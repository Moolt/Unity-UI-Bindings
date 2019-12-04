namespace UiBinding.Conversion
{
    public class UppercaseConverter : ValueConverter<string, string>
    {
        public override string Convert(string value)
        {
            return value?.ToUpperInvariant() ?? string.Empty;
        }
    }
}