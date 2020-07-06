using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerListItemViewModelFactory
    {
        IAimTimerListItemViewModel Create(IAimTimerItem aimTimerItem);
    }
}
