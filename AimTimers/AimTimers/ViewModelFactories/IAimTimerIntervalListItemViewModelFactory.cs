using AimTimers.Models;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerIntervalListItemViewModelFactory
    {
        IAimTimerIntervalListItemViewModel Create(AimTimerIntervalModel interval);
    }
}
