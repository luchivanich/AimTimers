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
            var currentAimTimerItem = GetAimTimerItemByDate(now);
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
            var lastInterval = GetAimTimerItemByDate(now).AimTimerItemModel.AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = now;
        }

        public TimeSpan GetTimeLeft()
        {
            var now = _dateTimeProvider.GetNow();
            var aimTimerItem = GetAimTimerItemByDate(now);
            aimTimerItem.Refresh();
            return new TimeSpan(AimTimerModel.Ticks ?? 0) - new TimeSpan(aimTimerItem.AimTimerItemModel.AimTimerIntervals?.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks) ?? 0);
        }

        private IAimTimerItem GetAimTimerItemByDate(DateTime date)
        {
            var aimTimerItemModel = AimTimerModel.AimTimerItemModels.FirstOrDefault(i => i.StartOfActivityPeriod.Date == date.Date);
            if (aimTimerItemModel == null)
            {
                aimTimerItemModel = AddAimTimerItem(date);
            }
            return new AimTimerItem(aimTimerItemModel);
        }

        private AimTimerItemModel AddAimTimerItem(DateTime date)
        {
            var aimTimerItem = new AimTimerItemModel(date.Date, date.Date.AddDays(1).AddTicks(-1));
            AimTimerModel.AimTimerItemModels.Add(aimTimerItem);
            return aimTimerItem;
        }
    }
}
