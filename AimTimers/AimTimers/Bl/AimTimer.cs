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

        public TimeSpan TimeLeft { get; private set; }

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
            var aimTimerItem = new AimTimerItemModel
            {
                AimTimerId = AimTimerModel.Id,
                StartOfActivityPeriod = date.Date,
                EndOfActivityPeriod = date.Date.AddDays(1).AddTicks(-1)
            };
            return aimTimerItem;
        }
    }
}
