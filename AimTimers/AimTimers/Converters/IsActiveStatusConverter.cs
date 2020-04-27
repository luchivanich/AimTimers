using System;
using System.Globalization;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.Converters
{
    public class IsActiveStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AimTimerStatusFlags statusFlags)
            {
                return (statusFlags & AimTimerStatusFlags.Active) == AimTimerStatusFlags.Active;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
