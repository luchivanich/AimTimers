using System;
using System.Linq;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimerItem : IAimTimerItem
    {
        public AimTimerItemModel AimTimerItemModel { get; }

        public AimTimerItem(AimTimerItemModel aimTimerItemModel)
        {
            AimTimerItemModel = aimTimerItemModel;
        }

        public void Refresh()
        {
            foreach (var item in AimTimerItemModel.AimTimerIntervals.Where(i => i.EndDate > AimTimerItemModel.EndOfActivityPeriod || (i.EndDate == null && DateTime.Now > AimTimerItemModel.EndOfActivityPeriod)))
            {
                item.EndDate = AimTimerItemModel.EndOfActivityPeriod;
            }
        }
    }
}
