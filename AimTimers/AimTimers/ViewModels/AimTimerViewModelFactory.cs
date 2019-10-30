using AimTimers.Models;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModelFactory : IAimTimerViewModelFactory
    {
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;

        public AimTimerViewModelFactory(IAimTimerService aimTimerService, INavigation navigation)
        {
            _navigation = navigation;
            _aimTimerService = aimTimerService;
        }

        public IAimTimerViewModel Create(AimTimer aimTimer)
        {
            var result = new AimTimerViewModel(_navigation, _aimTimerService);
            result.Setup(aimTimer);
            return result;
        }

        public IAimTimerViewModel CreateNew()
        {
            var aimTimer = new AimTimer();
            return Create(aimTimer);
        }
    }
}
