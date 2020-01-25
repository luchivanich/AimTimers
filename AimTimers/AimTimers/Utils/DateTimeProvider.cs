using System;

namespace AimTimers.Utils
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
