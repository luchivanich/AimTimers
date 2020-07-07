using System;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModels;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerListItemViewModelFactory : IAimTimerListItemViewModelFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAlertManager _alertManager;
        private readonly INavigation _navigation;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IAimTimerService _aimTimerService;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;
        private readonly Func<DateTime, DateTime?, IAimTimerInterval> _aimTimerIntervalFactory;
        private readonly Func<IAimTimerItem, IAimTimerInterval, IAimTimerIntervalViewModel> _aimTimerIntervalViewModelFactory;

        public AimTimerListItemViewModelFactory(
            IDateTimeProvider dateTimeProvider,
            IAlertManager alertManager,
            INavigation navigation,
            IMessagingCenter messagingCenter,
            IAimTimerService aimTimerService,
            IViewFactory viewFactory,
            IAimTimerIntervalListItemViewModelFactory aimTimerIntervalListItemViewModelFactory,
            Func<DateTime, DateTime?, IAimTimerInterval> aimTimerIntervalFactory,
            Func<IAimTimerItem, IAimTimerInterval, IAimTimerIntervalViewModel> aimTimerIntervalViewModelFactory)
        {
            _dateTimeProvider = dateTimeProvider;
            _alertManager = alertManager;
            _navigation = navigation;
            _messagingCenter = messagingCenter;
            _aimTimerService = aimTimerService;
            _viewFactory = viewFactory;
            _aimTimerIntervalListItemViewModelFactory = aimTimerIntervalListItemViewModelFactory;
            _aimTimerIntervalFactory = aimTimerIntervalFactory;
            _aimTimerIntervalViewModelFactory = aimTimerIntervalViewModelFactory;
        }

        public IAimTimerListItemViewModel Create(IAimTimerItem aimTimerItem)
        {
            var result = new AimTimerListItemViewModel(
                _dateTimeProvider,
                _alertManager,
                _navigation,
                _messagingCenter,
                _aimTimerService,
                _viewFactory,
                _aimTimerIntervalListItemViewModelFactory,
                _aimTimerIntervalFactory,
                _aimTimerIntervalViewModelFactory);
            result.Setup(aimTimerItem);
            return result;
        }
    }
}
