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

        public IAimTimerItemViewModel Create(IAimTimer aimTimer)
        {
            var result = new AimTimerItemViewModel(_aimTimerService);
            result.Setup(aimTimer);
            return result;
        }
    }
}
