using AimTimers.Models;

namespace AimTimers.Bl
{
    public interface IAimTimerInterval
    {
        IAimTimerItem AimTimerItem { get; }
        AimTimerIntervalModel AimTimerIntervalModel { get; }
    }
}
