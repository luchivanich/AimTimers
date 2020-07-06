using System;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public interface IAimTimer
    {
        AimTimerModel AimTimerModel { get; }
    }
}
