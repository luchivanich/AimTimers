using System;
using System.Linq;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimer : IAimTimer
    {
        public AimTimerModel AimTimerModel { get; }

        public AimTimer(AimTimerModel aimTimerModel)
        {
            AimTimerModel = aimTimerModel;
        }

        public void Start()
        {
            var now = DateTime.Now;
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
            var lastInterval = GetAimTimerItemByDate(DateTime.Now).AimTimerItemModel.AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = DateTime.Now;
        }

        public IAimTimerItem GetAimTimerItemByDate(DateTime date)
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
