using System;
using System.Collections.Generic;
using System.Linq;

namespace AimTimers.Models
{
    public class AimTimerItem
    {
        public int Id { get; set; }

        public List<AimTimerInterval> AimTimerIntervals { get; set; } = new List<AimTimerInterval>();

        public DateTime StartOfActivityPeriod { get; private set; }

        public DateTime EndOfActivityPeriod { get; private set; }

        public AimTimerItem(DateTime startOfActivityPeriod, DateTime endOfActivityPeriod)
        {
            StartOfActivityPeriod = startOfActivityPeriod;
            EndOfActivityPeriod = endOfActivityPeriod;
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
            if (now < StartOfActivityPeriod ||  now > EndOfActivityPeriod || AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            AimTimerIntervals.Add(new AimTimerInterval { AimTimerItem = this, StartDate = now, EndDate = null });
        }

        public void Refresh()
        {
            foreach(var item in AimTimerIntervals.Where(i => i.EndDate > EndOfActivityPeriod || (i.EndDate == null && DateTime.Now > EndOfActivityPeriod)))
            {
                item.EndDate = EndOfActivityPeriod;
            }
        }

    }
}
