using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerIntervalListItemViewModelFactory
    {
        IAimTimerIntervalListItemViewModel Create(IAimTimerInterval interval, IAimTimerListItemViewModel parent);
    }
}
