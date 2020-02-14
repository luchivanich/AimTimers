using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimer
    {
        AimTimerModel AimTimerModel { get; }

        void Start();
        void Stop();

        IAimTimerItem GetAimTimerItemByDate(DateTime date);
    }
}
