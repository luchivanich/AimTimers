using System;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public interface IAimTimer
    {
        AimTimerModel AimTimerModel { get; }
        void Start();
        void Stop();
        TimeSpan GetTimeLeft();
        IAimTimerItem GetAimTimerItemByDate(DateTime date);
        AimTimerStatus GetAimTimerStatus(DateTime date);
    }
}
