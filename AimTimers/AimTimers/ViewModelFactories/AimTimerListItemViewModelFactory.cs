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
        private readonly IAlertManager _alertManager;
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;
        private readonly Func<IAimTimer, AimTimerIntervalModel, IAimTimerInterval> _aimTimerIntervalFactory;
        private readonly Func<IAimTimerInterval, IAimTimerIntervalViewModel> _aimTimerIntervalViewModelFactory;

        public AimTimerListItemViewModelFactory(
            IAlertManager alertManager,
            INavigation navigation,
            IAimTimerService aimTimerService,
            IViewFactory viewFactory,
            IAimTimerIntervalListItemViewModelFactory aimTimerIntervalListItemViewModelFactory,
            Func<IAimTimer, AimTimerIntervalModel, IAimTimerInterval> aimTimerIntervalFactory,
            Func<IAimTimerInterval, IAimTimerIntervalViewModel> aimTimerIntervalViewModelFactory)
        {
            _alertManager = alertManager;
            _navigation = navigation;
            _aimTimerService = aimTimerService;
            _viewFactory = viewFactory;
            _aimTimerIntervalListItemViewModelFactory = aimTimerIntervalListItemViewModelFactory;
            _aimTimerIntervalFactory = aimTimerIntervalFactory;
            _aimTimerIntervalViewModelFactory = aimTimerIntervalViewModelFactory;
        }

        public IAimTimerListItemViewModel Create(IAimTimer aimTimer)
        {
            var result = new AimTimerListItemViewModel(
                _alertManager,
                _navigation,
                _aimTimerService,
                _viewFactory,
                _aimTimerIntervalListItemViewModelFactory,
                _aimTimerIntervalFactory,
                _aimTimerIntervalViewModelFactory);
            result.Setup(aimTimer);
            return result;
        }
    }
}
