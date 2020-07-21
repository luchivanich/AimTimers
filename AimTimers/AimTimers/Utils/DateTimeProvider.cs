using System;

namespace AimTimers.Utils
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private DateTime? _now;

        public bool IsToday => _now == null;

        public DateTime GetNow()
        {
            var result = _now ?? DateTime.Now;
            return new DateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second);
        }

        public void SetNow(DateTime? dateTime)
        {
            _now = dateTime;
        }
    }
}
