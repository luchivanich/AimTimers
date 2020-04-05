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
        void RefreshTimeLeft();
        IAimTimerItem GetCurrentAimTimerItem();
        AimTimerRunningStatus GetAimTimerRunningStatus();
        AimTimerStatus GetAimTimerStatus();
        void SetIsCanceled(bool isCanceled);
    }
}
