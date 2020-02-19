using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerItemViewModelFactory
    {
        IAimTimerItemViewModel Create(IAimTimer aimTimer);
    }
}
