public abstract class ValueConverter<TSource, TTarget> : IValueConverter<TSource, TTarget>
{
    public abstract TTarget Convert(TSource value);

    public virtual TTarget ConvertBack(TSource value)
    {
        throw new System.NotImplementedException();
    }

    public object Convert(object value)
    {
        return Convert((TSource)value);
    }

    public object ConvertBack(object value)
    {
        return ConvertBack((TTarget)value);
    }
}