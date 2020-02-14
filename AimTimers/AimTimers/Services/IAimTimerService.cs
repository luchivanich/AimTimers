using System.Collections.Generic;
using AimTimers.Bl;
using AimTimers.Models;

namespace AimTimers.Services
{
    public interface IAimTimerService
    {
        void AddAimTimer(AimTimerModel aimTimer);
        IEnumerable<IAimTimer> GetActiveAimTimers();
    }
}
