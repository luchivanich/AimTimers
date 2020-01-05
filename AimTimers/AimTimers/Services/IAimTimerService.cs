using System.Collections.Generic;
using AimTimers.Models;

namespace AimTimers.Services
{
    public interface IAimTimerService
    {
        IEnumerable<AimTimer> GetActiveAimTimers();

        void AddAimTimer(AimTimer aimTimer);
    }
}
