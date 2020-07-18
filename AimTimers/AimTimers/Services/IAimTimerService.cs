using System.Collections.Generic;
using AimTimers.Bl;
using AimTimers.Models;

namespace AimTimers.Services
{
    public interface IAimTimerService
    {
        void SaveAimTimer(IAimTimerItem aimTimerItem);
        IEnumerable<IAimTimerItem> GetActiveAimTimers();
        void DeleteAimTimer(IAimTimer aimTimer);
    }
}
