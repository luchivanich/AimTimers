using System;

namespace AimTimers.Utils
{
    [Flags]
    public enum AimTimerStatusFlags
    {
        None = 0,
        Active = 1,
        Running = 2
    }
}
