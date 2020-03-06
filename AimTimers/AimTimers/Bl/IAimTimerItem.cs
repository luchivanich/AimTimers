using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimerItem
    {
        AimTimerItemModel AimTimerItemModel { get; }
        void Refresh();
        TimeSpan GetTimeLeft();
    }
}
