using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerItemListItemViewModelFactory
    {
        IAimTimerItemListItemViewModel Create(IAimTimer aimTimer, IAimTimerItem aimTimerItem);
    }
}
