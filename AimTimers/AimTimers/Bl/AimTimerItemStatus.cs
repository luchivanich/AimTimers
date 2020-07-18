using System;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public struct AimTimerItemStatus
    {
        public TimeSpan TimeLeft { get; set; }
        public bool IsFinished { get; set; }
        public int InARow { get; set; }
        public AimTimerStatusFlags StatusFlags { get; set; }
    }
}
