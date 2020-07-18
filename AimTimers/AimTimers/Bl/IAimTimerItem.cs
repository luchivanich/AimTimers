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
        bool IsFinished { get; set; }
        int InARow { get; set; }

        AimTimerItemStatus GetStatus();
        void Start();
        void Stop();
        AimTimerItemModel GetAimTimerItemModel();
    }
}
