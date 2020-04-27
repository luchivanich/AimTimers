using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerIntervalListItemViewModelFactory : IAimTimerIntervalListItemViewModelFactory
    {
        public IAimTimerIntervalListItemViewModel Create(IAimTimerInterval interval, IAimTimerListItemViewModel parent)
        {
            return new AimTimerIntervalListItemViewModel
            {
                AimTimerInterval = interval,
                Parent = parent
            };
        }
    }
}
