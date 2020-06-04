using System;
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
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IAlertManager _alertManager;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerService _aimTimerService;
        private readonly IAimTimerListItemViewModelFactory _aimTimerItemViewModelFactory;
        private readonly IAimTimerViewModelFactory _aimTimerViewModelFactory;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;

        public ObservableCollection<IAimTimerListItemViewModel> AimTimerListItemViewModels { get; set; } = new ObservableCollection<IAimTimerListItemViewModel>();

        public string Title => _dateTimeProvider.GetNow().ToShortDateString();

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
                AimTimerListItemViewModels.Remove(aimTimerListItemViewModel);
            }
        }

        public ICommand GoDayBeforeCommand
        {
            get
            {
                return new Command(() => ExecuteGoDayBeforeCommand());
            }
        }

        private void ExecuteGoDayBeforeCommand()
        {
            var now = _dateTimeProvider.GetNow();
            _dateTimeProvider.SetNow(now.AddDays(-1));
            Init();
        }

        public ICommand GoNextDayCommand
        {
            get
            {
                return new Command(() => ExecuteGoNextDayCommand());
            }
        }

        private void ExecuteGoNextDayCommand()
        {
            var now = _dateTimeProvider.GetNow();
            _dateTimeProvider.SetNow(now.AddDays(1));
            Init();
        }

        public ICommand GoTodayCommand
        {
            get
            {
                return new Command(() => ExecuteGoTodayCommand());
            }
        }

        private void ExecuteGoTodayCommand()
        {
            _dateTimeProvider.SetNow(null);
            Init();
        }

        #endregion

        public AimTimersViewModel(
            IDateTimeProvider dateTimeProvider,
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
            _dateTimeProvider = dateTimeProvider;
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
            _aimTimerNotificationService.Stop();
            AimTimerListItemViewModels.CollectionChanged -= AimTimerItemViewModels_CollectionChanged;
            _aimTimerNotificationService.OnStatusChanged -= AimTimerNotificationService_OnStatusChanged;
            _messagingCenter.Unsubscribe<IAimTimer>(this, MessagingCenterMessages.AimTimerUpdated);

            AimTimerListItemViewModels.Clear();
            foreach (var aimTimerModel in _aimTimerService.GetActiveAimTimers())
            {
                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                AimTimerListItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimer));
            }

            _aimTimerNotificationService.SetItemsToFollow(AimTimerListItemViewModels.Select(i => i.GetAimTimer()).ToList());
            AimTimerListItemViewModels.CollectionChanged += AimTimerItemViewModels_CollectionChanged;
            _aimTimerNotificationService.OnStatusChanged += AimTimerNotificationService_OnStatusChanged;
            _aimTimerNotificationService.Start();
            _messagingCenter.Subscribe<IAimTimer>(this, MessagingCenterMessages.AimTimerUpdated, OnItemUpdated);

            OnPropertyChanged(nameof(Title));
        }

        private void OnItemUpdated(IAimTimer aimTimer)
        {
            var result = AimTimerListItemViewModels.FirstOrDefault(i => i.GetAimTimer() == aimTimer);
            if (result == null)
            {
                _aimTimerNotificationService.Stop();
                result = _aimTimerItemViewModelFactory.Create(aimTimer);
                AimTimerListItemViewModels.Add(result);
                _aimTimerNotificationService.Start();
            }
            result.Refresh();
        }

        private void AimTimerItemViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _aimTimerNotificationService.SetItemsToFollow(AimTimerListItemViewModels.Select(i => i.GetAimTimer()).ToList());
        }

        private void AimTimerNotificationService_OnStatusChanged(object sender, AimTimersEventArgs e)
        {
            foreach (var aimTimerItemViewModel in AimTimerListItemViewModels)
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