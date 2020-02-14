using System;

namespace AimTimers.Models
{
    public class AimTimerIntervalModel
    {
        public string Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
