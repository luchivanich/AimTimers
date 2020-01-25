using System;
using System.Collections.Generic;
using AimTimers.Models;

namespace AimTimers.Services
{
    public interface IAimTimerService
    {
        void Stop(AimTimer aimTimer);
        void Start(AimTimer aimTimer);
        
        void AddAimTimer(AimTimer aimTimer);
        IEnumerable<AimTimer> GetActiveAimTimers();
    }
}
