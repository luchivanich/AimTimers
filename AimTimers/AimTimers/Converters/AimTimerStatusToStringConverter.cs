using System;
using System.Globalization;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.Converters
{
    public class AimTimerStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AimTimerStatus status))
            {
                return string.Empty;
            }
            switch (status)
            {
                case AimTimerStatus.Active: return "Active";
                case AimTimerStatus.Canceled: return "Canceled";
                case AimTimerStatus.Finished: return "Finished";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
