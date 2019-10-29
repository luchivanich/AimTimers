using AimTimers.Models;
using AimTimers.Services;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModelFactory : IAimTimerItemViewModelFactory
    {
        private readonly IAimTimerTickService _aimTimerService;

        public AimTimerItemViewModelFactory(IAimTimerTickService aimTimerService)
        {
            _aimTimerService = aimTimerService;
        }

        public IAimTimerItemViewModel Create(AimTimerItem aimTimerItem)
        {
            var result = new AimTimerItemViewModel(_aimTimerService);
            result.Setup(aimTimerItem);
            return result;
        }

        public IAimTimerItemViewModel CreateNew()
        {
            var aimTimerItem = new AimTimerItem
            {
                AimTimer = new AimTimer()
            };
            return Create(aimTimerItem);
        }
    }
}
