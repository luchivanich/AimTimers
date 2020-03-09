using AimTimers.Models;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerIntervalListItemViewModelFactory : IAimTimerIntervalListItemViewModelFactory
    {
        public IAimTimerIntervalListItemViewModel Create(AimTimerIntervalModel interval)
        {
            return new AimTimerIntervalListItemViewModel
            {
                StartDate = interval.StartDate.ToLongTimeString(),
                EndDate = interval.EndDate?.ToLongTimeString()
            };
        }
    }
}
