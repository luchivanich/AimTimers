using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerListItemViewModelFactory : IAimTimerListItemViewModelFactory
    {
        private readonly IAimTimerService _aimTimerService;

        public AimTimerListItemViewModelFactory(IAimTimerService aimTimerService)
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
