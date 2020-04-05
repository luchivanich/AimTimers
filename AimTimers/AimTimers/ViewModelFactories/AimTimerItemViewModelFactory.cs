using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
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

        public IAimTimerListItemViewModel Create(IAimTimer aimTimer)
        {
            var result = new AimTimerListItemViewModel(_aimTimerService);
            result.Setup(aimTimer);
            return result;
        }
    }
}
