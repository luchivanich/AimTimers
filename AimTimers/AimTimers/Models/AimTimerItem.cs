using System;
using System.Collections.Generic;
using System.Linq;

namespace AimTimers.Models
{
    public class AimTimerItem
    {
        public int Id { get; set; }

        public AimTimer AimTimer { get; set; }

        public List<AimTimerInterval> AimTimerIntervals { get; set; } = new List<AimTimerInterval>();

        public AimTimerItemStatus Status { get; set; }

        public DateTime? StartOfActivityPeriod { get; set; }

        public DateTime? EndOfActivityPeriod { get; set; }

        public TimeSpan TimeLeft => GetTimeLeft();

        private TimeSpan GetTimeLeft()
        {
            Refresh();
            return (AimTimer.Time ?? new TimeSpan()) - new TimeSpan(AimTimerIntervals?.Sum(i => (i.EndDate ?? DateTime.Now).Ticks - i.StartDate.Ticks) ?? 0);
        }

        public void Pause()
        {
            var lastInterval = AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = DateTime.Now;
        }

        public void Start()
        {
            var now = DateTime.Now;
            if ((StartOfActivityPeriod.HasValue && now < StartOfActivityPeriod) || 
                (EndOfActivityPeriod.HasValue && now > EndOfActivityPeriod ) || 
                AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            AimTimerIntervals.Add(new AimTimerInterval { AimTimerItem = this, StartDate = now, EndDate = null });
        }

        private void Refresh()
        {
            foreach(var item in AimTimerIntervals.Where(i => i.EndDate > EndOfActivityPeriod || (i.EndDate == null && DateTime.Now > EndOfActivityPeriod)))
            {
                item.EndDate = EndOfActivityPeriod;
            }
        }

    }
}
