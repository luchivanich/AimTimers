using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerItemViewModelFactory : IAimTimerItemViewModelFactory
    {
        private readonly IAimTimerService _aimTimerService;

        public AimTimerItemViewModelFactory(IAimTimerService aimTimerService)
        {
            _aimTimerService = aimTimerService;
        }

        public IAimTimerItemViewModel Create(IAimTimer aimTimer, IAimTimerItem aimTimerItem)
        {
            var result = new AimTimerItemViewModel(_aimTimerService);
            result.Setup(aimTimer, aimTimerItem);
            return result;
        }
    }
}
