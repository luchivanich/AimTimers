using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Models;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private List<AimTimer> _aimTimers = new List<AimTimer>();

        public AimTimerService()
        {
        }

        public IEnumerable<AimTimerItem> GetActiveAimTimerItems()
        {
            var today = DateTime.Today;
            var result = new List<AimTimerItem>();
            foreach (var aimTimer in _aimTimers)
            {
                var aimTimerItem = aimTimer.AimTimerItems.FirstOrDefault(i => i.StartOfActivityPeriod.Value.Date == today);
                if (aimTimerItem == null)
                {
                    aimTimerItem = aimTimer.AddAimTimerItem(today);
                }
                result.Add(aimTimerItem);
            }
            return result;
        }

        public void AddAimTimer(AimTimer aimTimer)
        {
            _aimTimers.Add(aimTimer);
        }
    }
}
