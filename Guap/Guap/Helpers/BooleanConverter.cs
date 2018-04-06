namespace Guap.Helpers
{
    using System;
    using System.Globalization;

    using Xamarin.Forms;

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (value is bool b ? !b : throw new InvalidOperationException("The target must be a boolean"));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}