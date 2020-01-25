using System;
using System.Collections.Generic;
using AimTimers.Models;

namespace AimTimers.Services
{
    public class AimTimersEventArgs : EventArgs
    {
        public IEnumerable<AimTimer> AimTimers { get; set; }
    }
}
