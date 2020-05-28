using System;
using System.Linq;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public class AimTimer : IAimTimer
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public AimTimerModel AimTimerModel { get; }

        public AimTimer(AimTimerModel aimTimerModel, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            AimTimerModel = aimTimerModel;
        }

        public void Start()
        {
            var now = _dateTimeProvider.GetNow();
            var currentAimTimerItem = GetCurrentAimTimerItem();
            if (currentAimTimerItem.AimTimerItemModel.IsCanceled)
            {
                return;
            }
            if (now < currentAimTimerItem.AimTimerItemModel.StartOfActivityPeriod || 
                now > currentAimTimerItem.AimTimerItemModel.EndOfActivityPeriod || 
                currentAimTimerItem.AimTimerItemModel.AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            currentAimTimerItem.AimTimerItemModel.AimTimerIntervals.Add(new AimTimerIntervalModel { StartDate = now, EndDate = null });
        }

        public void Stop()
        {
            var now = _dateTimeProvider.GetNow();
            var currentAimTimerItem = GetCurrentAimTimerItem();
            var lastInterval = currentAimTimerItem.AimTimerItemModel.AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = now;
        }

        public TimeSpan TimeLeft { get; private set; }
        public bool IsDeleted { get; set; }

        public void RefreshTimeLeft()
        {
            var now = _dateTimeProvider.GetNow();
            var aimTimerItem = GetCurrentAimTimerItem();
            aimTimerItem.Refresh();
            TimeLeft = new TimeSpan(AimTimerModel.Ticks ?? 0) - new TimeSpan(aimTimerItem.AimTimerItemModel.AimTimerIntervals?.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks) ?? 0);
        }

        public IAimTimerItem GetCurrentAimTimerItem()
        {
            var now = _dateTimeProvider.GetNow();
            var aimTimerItemModel = AimTimerModel.AimTimerItemModels.FirstOrDefault(i => i.StartOfActivityPeriod <= now && i.EndOfActivityPeriod >= now);
            if (aimTimerItemModel == null)
            {
                aimTimerItemModel = AddAimTimerItem(now);
            }
            return new AimTimerItem(this, aimTimerItemModel, _dateTimeProvider);
        }

        private AimTimerItemModel AddAimTimerItem(DateTime date)
        {
            var aimTimerItem = new AimTimerItemModel(date.Date, date.Date.AddDays(1).AddTicks(-1));
            AimTimerModel.AimTimerItemModels.Add(aimTimerItem);
            return aimTimerItem;
        }

        public AimTimerStatusFlags GetAimTimerStatusFlags()
        {
            var result = AimTimerStatusFlags.None;
            var now = _dateTimeProvider.GetNow();
            var aimTimerItem = GetCurrentAimTimerItem();
            aimTimerItem.Refresh();
            var interval = aimTimerItem.AimTimerItemModel.AimTimerIntervals.FirstOrDefault(i => i.StartDate <= now && i.EndDate >= now || i.EndDate == null);
            if (interval != null && interval.EndDate == null)
            {
                result |= AimTimerStatusFlags.Running;
            }
            if (TimeLeft.Ticks > 0)
            {
                result |= AimTimerStatusFlags.Active;
            }
            return result;
        }

        public void SetIsCanceled(bool isCanceled)
        {
            if (isCanceled)
            {
                Stop();
            }
            GetCurrentAimTimerItem().AimTimerItemModel.IsCanceled = isCanceled;
        }
    }
}
