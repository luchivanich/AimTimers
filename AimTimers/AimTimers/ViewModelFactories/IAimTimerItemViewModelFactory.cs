using AimTimers.Models;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerItemViewModelFactory
    {
        IAimTimerItemViewModel Create(AimTimer aimTimer, AimTimerItem aimTimerItem);
    }
}
