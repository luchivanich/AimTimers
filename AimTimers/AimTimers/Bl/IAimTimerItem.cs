using System;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public interface IAimTimerItem
    {
        IAimTimer AimTimer { get; }
        AimTimerModel AimTimerModel { get; }
        AimTimerItemModel AimTimerItemModel { get; }
        long Ticks { get; set; }
        void Refresh();
        TimeSpan GetTimeLeft();
        void Start();
        void Stop();
        AimTimerStatusFlags GetAimTimerStatusFlags();
    }
}
