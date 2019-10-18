using System;

namespace AimTimers.Models
{
    public class AimTimerInterval
    {
        public string Id { get; set; }

        public AimTimerItem AimTimerItem { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
