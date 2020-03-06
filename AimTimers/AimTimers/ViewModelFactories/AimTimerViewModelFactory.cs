using System;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.ViewModels;
using Xamarin.Forms;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerViewModelFactory : IAimTimerViewModelFactory
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
        Func<AimTimerModel, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;
        private readonly IAimTimerItemListItemViewModelFactory _aimTimerItemListItemViewModelFactory;

        public AimTimerViewModelFactory(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IAimTimerService aimTimerService,
            Func<AimTimerModel, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory,
            IAimTimerItemListItemViewModelFactory aimTimerItemListItemViewModelFactory)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
            _navigation = navigation;
            _aimTimerService = aimTimerService;
            _aimTimerItemFactory = aimTimerItemFactory;
            _aimTimerItemListItemViewModelFactory = aimTimerItemListItemViewModelFactory;
        }

        public IAimTimerViewModel Create(IAimTimer aimTimer)
        {
            var result = new AimTimerViewModel(_aimTimerNotificationService, _navigation, _aimTimerService, _aimTimerItemFactory, _aimTimerItemListItemViewModelFactory);
            result.Setup(aimTimer);
            return result;
        }
    }
}
