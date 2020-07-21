using System;
using System.Collections.Generic;

namespace AimTimers.Models
{
    public class AimTimerItemModel : BaseModel, IModel
    {
        public string AimTimerId { get; set; }
        public long Ticks { get; set; }
        public List<AimTimerIntervalModel> AimTimerIntervals { get; set; } = new List<AimTimerIntervalModel>();
        public DateTime StartOfActivityPeriod { get; set; }
        public DateTime EndOfActivityPeriod { get; set; }
        public bool IsFinished { get; set; }
        public int PreviousInARow { get; set; }
        public int Index { get; set; }
    }
}
