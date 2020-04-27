using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimerInterval : IAimTimerInterval
    {
        public AimTimerIntervalModel AimTimerIntervalModel { get; }
        public IAimTimer AimTimer { get; }

        public AimTimerInterval(IAimTimer aimTimer, AimTimerIntervalModel aimTimerIntervalModel)
        {
            AimTimer = aimTimer;
            AimTimerIntervalModel = aimTimerIntervalModel;
        }
    }
}
