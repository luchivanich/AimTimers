using System;

namespace AimTimers.Utils
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();

        void SetNow(DateTime? dateTime);
    }
}
