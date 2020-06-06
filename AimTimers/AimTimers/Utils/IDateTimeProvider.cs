using System;

namespace AimTimers.Utils
{
    public interface IDateTimeProvider
    {
        bool IsToday { get; }

        DateTime GetNow();

        void SetNow(DateTime? dateTime);
    }
}
