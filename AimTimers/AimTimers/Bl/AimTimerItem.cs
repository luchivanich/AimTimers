using System;
using System.Linq;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public class AimTimerItem : IAimTimerItem
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public IAimTimer AimTimer { get; }
        public AimTimerModel AimTimerModel { get; }
        public AimTimerItemModel AimTimerItemModel { get; }
        public long Ticks { get; set; }

        public AimTimerItem(IAimTimer aimTimer, AimTimerItemModel aimTimerItemModel, IDateTimeProvider dateTimeProvider)
        {
            AimTimer = aimTimer;
            AimTimerModel = AimTimer.AimTimerModel;
            AimTimerItemModel = aimTimerItemModel;
            _dateTimeProvider = dateTimeProvider;
        }

        public void Refresh()
        {
            foreach (var item in AimTimerItemModel.AimTimerIntervals.Where(i => i.EndDate > AimTimerItemModel.EndOfActivityPeriod || (i.EndDate == null && DateTime.Now > AimTimerItemModel.EndOfActivityPeriod)))
            {
                item.EndDate = AimTimerItemModel.EndOfActivityPeriod;
            }
        }

        public TimeSpan GetTimeLeft()
        {
            var now = _dateTimeProvider.GetNow();
            Refresh();
            return new TimeSpan(AimTimerItemModel.Ticks ?? 0) - new TimeSpan(AimTimerItemModel.AimTimerIntervals?.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks) ?? 0);
        }

        public void Start()
        {
            var now = _dateTimeProvider.GetNow();
            if (now < AimTimerItemModel.StartOfActivityPeriod ||
                now > AimTimerItemModel.EndOfActivityPeriod ||
                AimTimerItemModel.AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            AimTimerItemModel.AimTimerIntervals.Add(new AimTimerIntervalModel { StartDate = now, EndDate = null });
        }

        public void Stop()
        {
            var now = _dateTimeProvider.GetNow();
            var lastInterval = AimTimerItemModel.AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = now;
        }

        public AimTimerStatusFlags GetAimTimerStatusFlags()
        {
            var result = AimTimerStatusFlags.None;
            var now = _dateTimeProvider.GetNow();
            Refresh();
            var interval = AimTimerItemModel.AimTimerIntervals.FirstOrDefault(i => i.StartDate <= now && i.EndDate >= now || i.EndDate == null);
            if (interval != null && interval.EndDate == null)
            {
                result |= AimTimerStatusFlags.Running;
            }
            if (GetTimeLeft().Ticks > 0)
            {
                result |= AimTimerStatusFlags.Active;
            }
            return result;
        }
    }
}
