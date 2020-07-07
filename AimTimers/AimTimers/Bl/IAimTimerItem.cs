using System;
using System.Collections.Generic;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public interface IAimTimerItem
    {
        IAimTimer AimTimer { get; }
        long Ticks { get; set; }
        List<IAimTimerInterval> AimTimerIntervals { get; set; }
        DateTime StartOfActivityPeriod { get; set; }
        DateTime EndOfActivityPeriod { get; set; }
        void Refresh();
        TimeSpan GetTimeLeft();
        void Start();
        void Stop();
        AimTimerStatusFlags GetAimTimerStatusFlags();
        AimTimerItemModel GetAimTimerItemModel();
    }
}
