using System;
using System.Collections.Generic;

namespace AimTimers.Models
{
    public class AimTimer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan? Time { get; set; }
        public List<AimTimerItem> AimTimerItems { get; set; }
    }
}