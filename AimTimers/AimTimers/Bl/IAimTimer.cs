using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimer
    {
        string Title { get; set; }
        long Ticks { get; set; }
        DateTime OriginDate { get; set; }
        AimTimerModel GetAimTimerModel();
    }
}
