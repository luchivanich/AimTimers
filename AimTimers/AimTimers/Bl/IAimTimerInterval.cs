using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimerInterval
    {
        IAimTimer AimTimer { get; }
        AimTimerIntervalModel AimTimerIntervalModel { get; }
    }
}
