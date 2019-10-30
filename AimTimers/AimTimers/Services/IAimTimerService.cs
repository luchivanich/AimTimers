using System.Collections.Generic;
using AimTimers.Models;

namespace AimTimers.Services
{
    public interface IAimTimerService
    {
        IEnumerable<AimTimerItem> GetActiveAimTimerItems();

        void AddAimTimer(AimTimer aimTimer);
    }
}
