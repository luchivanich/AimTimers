using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.ViewModels;
using Xamarin.Forms;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerViewModelFactory : IAimTimerViewModelFactory
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IAimTimerService _aimTimerService;

        public AimTimerViewModelFactory(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IMessagingCenter messagingCenter,
            IAimTimerService aimTimerService)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
            _navigation = navigation;
            _messagingCenter = messagingCenter;
            _aimTimerService = aimTimerService;
        }

        public IAimTimerViewModel Create(IAimTimer aimTimer)
        {
            var result = new AimTimerViewModel(
                _aimTimerNotificationService,
                _navigation,
                _messagingCenter,
                _aimTimerService);
            result.Setup(aimTimer);
            return result;
        }
    }
}
