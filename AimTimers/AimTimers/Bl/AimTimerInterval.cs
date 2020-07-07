using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimerInterval : IAimTimerInterval
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public AimTimerIntervalModel GetAimTimerIntervalModel()
        {
            return new AimTimerIntervalModel
            {
                StartDate = StartDate,
                EndDate = EndDate
            };
        }
    }
}
