using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimerInterval
    {
        AimTimerIntervalModel AimTimerIntervalModel { get; }

        DateTime StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
