﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModelFactories;
using AimTimers.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimersViewModel : BaseViewModel, IAimTimersViewModel
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IAlertManager _alertManager;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerService _aimTimerService;
        private readonly IAimTimerListItemViewModelFactory _aimTimerItemViewModelFactory;
        private readonly IAimTimerViewModelFactory _aimTimerViewModelFactory;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;

        private bool _isLoaded;

        public ObservableCollection<IAimTimerListItemViewModel> AimTimerItemViewModels { get; set; } = new ObservableCollection<IAimTimerListItemViewModel>();

        public string Title => "Active Timers";

        #region Commands

        public ICommand AddItemCommand
        {
            get
            {
                return new Command(async () => await ExecuteAddItemCommand());
            }
        }

        private async Task ExecuteAddItemCommand()
        {
            var aimTimer = _aimTimerFactory.Invoke(new AimTimerModel());
            await NavigateAimTimerView(aimTimer);
        }

        public ICommand EditItemCommand
        {
            get
            {
                return new Command<AimTimerListItemViewModel>(async (aimTimerListItemViewModel) => await ExecuteEditItemCommand(aimTimerListItemViewModel));
            }
        }

        private async Task ExecuteEditItemCommand(IAimTimerListItemViewModel aimTimerListItemViewModel)
        {
            await NavigateAimTimerView(aimTimerListItemViewModel.GetAimTimer());
        }

        public ICommand DeleteItemCommand
        {
            get
            {
                return new Command<AimTimerListItemViewModel>(async (aimTimerListItemViewModel) => await ExecuteDeleteItemCommand(aimTimerListItemViewModel));
            }
        }

        private async Task ExecuteDeleteItemCommand(IAimTimerListItemViewModel aimTimerListItemViewModel)
        {
            if (await _alertManager.DisplayAlert("Warning!", "Would you like to remove the timer completely?", "Yes", "No"))
            {
                _aimTimerService.DeleteAimTimer(aimTimerListItemViewModel.GetAimTimer()?.AimTimerModel.Id);
                AimTimerItemViewModels.Remove(aimTimerListItemViewModel);
            }
        }

        #endregion

        public AimTimersViewModel(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IAlertManager alertManager,
            IMessagingCenter messagingCenter,
            IViewFactory viewFactory,
            IAimTimerService aimTimerService,
            IAimTimerListItemViewModelFactory aimTimerItemViewModelFactory,
            IAimTimerViewModelFactory aimTimerViewModelFactory,
            Func<AimTimerModel, IAimTimer> aimTimerFactory)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
            _navigation = navigation;
            _alertManager = alertManager;
            _messagingCenter = messagingCenter;
            _viewFactory = viewFactory;
            _aimTimerService = aimTimerService;
            _aimTimerItemViewModelFactory = aimTimerItemViewModelFactory;
            _aimTimerViewModelFactory = aimTimerViewModelFactory;
            _aimTimerFactory = aimTimerFactory;
        }

        public void Init()
        {
            if (!_isLoaded)
            {
                InitialLoad();
                _isLoaded = true;
            }
            else
            {
                var itemsToRemove = AimTimerItemViewModels.Where(i => i.GetAimTimer().IsDeleted).ToList();
                foreach (var item in itemsToRemove)
                {
                    AimTimerItemViewModels.Remove(item);
                    _aimTimerNotificationService.Remove(item.GetAimTimer());
                }

                foreach (var i in AimTimerItemViewModels)
                {
                    i.Refresh();
                }
            }
            _aimTimerNotificationService.OnStatusChanged += _aimTimerNotificationService_OnStatusChanged;
            _messagingCenter.Subscribe<string>(this, "AimTimerUpdated", OnItemAdded);
        }

        private void OnItemAdded(string s)
        {
            InitialLoad();
        }

        private void InitialLoad()
        {
            AimTimerItemViewModels.Clear();
            foreach (var aimTimerModel in _aimTimerService.GetActiveAimTimers())
            {
                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                AimTimerItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimer));
            }

            _aimTimerNotificationService.SetItemsToFollow(AimTimerItemViewModels.Select(i => i.GetAimTimer()).ToList());
            _aimTimerNotificationService.Start();
            AimTimerItemViewModels.CollectionChanged += AimTimerItemViewModels_CollectionChanged;
        }

        private void AimTimerItemViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _aimTimerNotificationService.SetItemsToFollow(AimTimerItemViewModels.Select(i => i.GetAimTimer()).ToList());
        }

        private void _aimTimerNotificationService_OnStatusChanged(object sender, AimTimersEventArgs e)
        {
            Console.Write(DateTime.Now);

            foreach (var aimTimerItemViewModel in AimTimerItemViewModels)
            {
                if (e.AimTimers.Any(i => i == aimTimerItemViewModel.GetAimTimer()))
                {
                    aimTimerItemViewModel.RefreshTimeLeft();
                }
            }
        }

        private async Task NavigateAimTimerView(IAimTimer aimTimer)
        {
            var aimTimerViewModel = _aimTimerViewModelFactory.Create(aimTimer);
            var aimTimerView = _viewFactory.CreatePopupPage(aimTimerViewModel);
            await _navigation.PushPopupAsync(aimTimerView);
        }
    }
}