using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimerInterval
    {
        DateTime StartDate { get; set; }
        DateTime? EndDate { get; set; }
        AimTimerIntervalModel GetAimTimerIntervalModel();
    }
}
