using AimTimers.Models;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerItemViewModelFactory : IAimTimerItemViewModelFactory
    {
        public IAimTimerItemViewModel Create(AimTimer aimTimer, AimTimerItem aimTimerItem)
        {
            var result = new AimTimerItemViewModel();
            result.Setup(aimTimer, aimTimerItem);
            return result;
        }
    }
}
