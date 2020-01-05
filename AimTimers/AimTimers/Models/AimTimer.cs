using System;
using System.Collections.Generic;
using System.Linq;

namespace AimTimers.Models
{
    public class AimTimer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? Ticks { get; set; }
        public List<AimTimerItem> AimTimerItems { get; set; } = new List<AimTimerItem>();

        public AimTimerItem GetAimTimerByDate(DateTime date)
        {
            var aimTimerItem = AimTimerItems.FirstOrDefault(i => i.StartOfActivityPeriod.Date == date.Date);
            if (aimTimerItem == null)
            {
                aimTimerItem = AddAimTimerItem(date);
            }
            return aimTimerItem;
        }

        private AimTimerItem AddAimTimerItem(DateTime date)
        {
            var aimTimerItem = new AimTimerItem(date.Date, date.Date.AddDays(1).AddTicks(-1));
            AimTimerItems.Add(aimTimerItem);
            return aimTimerItem;
        }
    }
}