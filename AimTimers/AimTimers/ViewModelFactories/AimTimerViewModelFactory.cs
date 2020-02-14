using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.ViewModels;
using Xamarin.Forms;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerViewModelFactory : IAimTimerViewModelFactory
    {
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;

        public AimTimerViewModelFactory(INavigation navigation, IAimTimerService aimTimerService)
        {
            _navigation = navigation;
            _aimTimerService = aimTimerService;
        }

        public IAimTimerViewModel Create(IAimTimer aimTimer)
        {
            var result = new AimTimerViewModel(_navigation, _aimTimerService);
            result.Setup(aimTimer);
            return result;
        }
    }
}
