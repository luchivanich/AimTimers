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
        public List<AimTimerItem> AimTimerItems { get; set; } = new List<AimTimerItem>();

        public AimTimerItem AddAimTimerItem(DateTime date)
        {
            var result = new AimTimerItem
            {
                AimTimer = this,
                StartOfActivityPeriod = date,
                EndOfActivityPeriod = date.AddDays(1).AddTicks(-1)
            };
            AimTimerItems.Add(result);
            return result;
        }
    }
}