using System;

namespace AimTimers.Utils
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private DateTime? _now;

        public bool IsToday => _now == null;

        public DateTime GetNow()
        {
            return _now ?? DateTime.Now;
        }

        public void SetNow(DateTime? dateTime)
        {
            _now = dateTime;
        }
    }
}
