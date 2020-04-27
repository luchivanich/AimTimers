using System;
using System.Globalization;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.Converters
{
    public class StatusFlagsToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AimTimerStatusFlags statusFlags)
            {
                if ((statusFlags & AimTimerStatusFlags.Active) != AimTimerStatusFlags.Active)
                {
                    return Color.FromHex("61f7ff");
                }

                if ((statusFlags & AimTimerStatusFlags.Running) == AimTimerStatusFlags.Running)
                {
                    return Color.FromHex("ffd24d");
                }

                return Color.FromHex("ffb2ae");
            }

            return Color.FromHex("ffb2ae");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
