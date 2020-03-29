using System;
using System.Globalization;
using Xamarin.Forms;

namespace AimTimers.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBool(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBool(value);
        }

        private bool? ConvertBool(object value)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return null;
        }
    }
}
