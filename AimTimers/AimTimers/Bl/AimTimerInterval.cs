using System.Linq;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimerInterval : IAimTimerInterval
    {
        public AimTimerIntervalModel AimTimerIntervalModel { get; }
        public IAimTimerItem AimTimerItem { get; }

        public AimTimerInterval(IAimTimerItem aimTimerItem,  AimTimerIntervalModel aimTimerIntervalModel)
        {
            AimTimerItem = aimTimerItem;
            AimTimerIntervalModel = aimTimerIntervalModel;
            if (aimTimerItem.AimTimerItemModel.AimTimerIntervals.All(i => i.Id != aimTimerIntervalModel.Id))
            {
                aimTimerItem.AimTimerItemModel.AimTimerIntervals.Add(aimTimerIntervalModel);
            }
        }
    }
}
