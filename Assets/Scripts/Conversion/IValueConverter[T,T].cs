public interface IValueConverter<TSource, TTarget> : IValueConverter
{
    TTarget Convert(TSource value);

    TTarget ConvertBack(TSource value);
}