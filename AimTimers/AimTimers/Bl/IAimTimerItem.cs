using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimerItem
    {
        IAimTimer AimTimer { get; }
        AimTimerItemModel AimTimerItemModel { get; }
        void Refresh();
        TimeSpan GetTimeLeft();
    }
}
