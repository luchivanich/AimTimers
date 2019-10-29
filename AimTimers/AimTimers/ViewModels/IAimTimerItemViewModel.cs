
using AimTimers.Models;

namespace AimTimers.ViewModels
{
    public interface IAimTimerItemViewModel
    {
        void RefreshTimeLeft();

        AimTimer GetAimTimer();
    }
}
