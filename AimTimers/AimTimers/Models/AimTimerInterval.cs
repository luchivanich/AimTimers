using System;
using Newtonsoft.Json;

namespace AimTimers.Models
{
    public class AimTimerInterval
    {
        public string Id { get; set; }

        [JsonIgnore]
        public AimTimerItem AimTimerItem { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
