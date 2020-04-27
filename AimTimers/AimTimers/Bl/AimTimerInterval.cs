using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimerInterval : IAimTimerInterval
    {
        public AimTimerIntervalModel AimTimerIntervalModel { get; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public AimTimerInterval(AimTimerIntervalModel aimTimerIntervalModel)
        {
            AimTimerIntervalModel = aimTimerIntervalModel;
        }
    }
}
