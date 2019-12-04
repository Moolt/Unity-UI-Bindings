using System;

namespace UiBinding.Conversion
{
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
}