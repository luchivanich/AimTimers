﻿using System;
using System.Globalization;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.Converters
{
    public class AimTimerStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AimTimerStatusFlags status))
            {
                return string.Empty;
            }

            if ((status & AimTimerStatusFlags.Active) == AimTimerStatusFlags.Active)
            {
                return "Active";
            }
            return "Finished";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
