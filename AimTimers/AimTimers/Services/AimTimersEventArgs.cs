using System;
using System.Collections.Generic;
using AimTimers.Bl;

namespace AimTimers.Services
{
    public class AimTimersEventArgs : EventArgs
    {
        public IEnumerable<IAimTimerItem> AimTimerItems { get; set; }
    }
}
