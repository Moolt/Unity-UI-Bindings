using System;
using UnityEngine;

public interface IValueConverter
{
    object Convert(object value);

    object ConvertBack(object value);
}

public interface IValueConverter<TSource, TTarget> : IValueConverter
{
    TTarget Convert(TSource value);

    TTarget ConvertBack(TSource value);
}

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

public class DefaultConverter : IValueConverter
{
    private readonly Type _source;
    private readonly Type _target;

    public DefaultConverter(Type source, Type target)
    {
        _source = source;
        _target = target;
    }

    public object Convert(object value)
    {
        return Convert(value, _target);
    }

    public object ConvertBack(object value)
    {
        return Convert(value, _source);
    }

    private object Convert(object value, Type to)
    {
        return System.Convert.ChangeType(value, to);
    }
}

public class ColorConverter : ValueConverter<float, Color>
{
    public override Color Convert(float value)
    {
        return Color.Lerp(Color.red, Color.blue, value);
    }
}
