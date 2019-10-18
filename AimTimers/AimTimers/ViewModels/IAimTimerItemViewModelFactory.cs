using AimTimers.Models;

namespace AimTimers.ViewModels
{
    public interface IAimTimerItemViewModelFactory
    {
        IAimTimerItemViewModel Create(AimTimerItem aimTimerItem);

        IAimTimerItemViewModel CreateNew();
    }
}
