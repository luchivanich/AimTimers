using System;
using System.Collections.Generic;

namespace AimTimers.Models
{
    public class AimTimerItemModel
    {
        public int Id { get; set; }

        public List<AimTimerIntervalModel> AimTimerIntervals { get; set; } = new List<AimTimerIntervalModel>();

        public DateTime StartOfActivityPeriod { get; private set; }

        public DateTime EndOfActivityPeriod { get; private set; }

        public AimTimerItemModel(DateTime startOfActivityPeriod, DateTime endOfActivityPeriod)
        {
            StartOfActivityPeriod = startOfActivityPeriod;
            EndOfActivityPeriod = endOfActivityPeriod;
        }
    }
}
