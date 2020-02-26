using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerItemViewModelFactory
    {
        IAimTimerListItemViewModel Create(IAimTimer aimTimer);
    }
}
