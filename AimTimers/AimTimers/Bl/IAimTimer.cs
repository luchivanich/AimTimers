using System;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public interface IAimTimer
    {
        AimTimerModel AimTimerModel { get; }
        void Start();
        void Stop();
        TimeSpan TimeLeft { get; }
        bool IsDeleted { get; set; }
        void RefreshTimeLeft();
        IAimTimerItem GetCurrentAimTimerItem();
        AimTimerStatusFlags GetAimTimerStatusFlags();
        void SetIsCanceled(bool isCanceled);
    }
}
