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
            return new TimeSpan(AimTimerModel.Ticks ?? 0) - new TimeSpan(AimTimerItemModel.AimTimerIntervals?.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks) ?? 0);
        }
    }
}
