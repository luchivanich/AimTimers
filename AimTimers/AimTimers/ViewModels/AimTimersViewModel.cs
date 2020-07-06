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
        private readonly Func<IAimTimer, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;

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
            var itemToAdd = new AimTimerItemModel
            {
                StartOfActivityPeriod = _dateTimeProvider.GetStartOfTheDay(),
                EndOfActivityPeriod = _dateTimeProvider.GetEndOfTheDay()
            };
            var aimTimerItem = _aimTimerItemFactory.Invoke(aimTimer, itemToAdd);
            await NavigateAimTimerView(aimTimerItem);
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
            await NavigateAimTimerView(aimTimerListItemViewModel.GetAimTimerItem());
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
                _aimTimerService.DeleteAimTimer(aimTimerListItemViewModel.GetAimTimerItem()?.AimTimerItemModel.AimTimerId);
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
            Func<AimTimerModel, IAimTimer> aimTimerFactory,
            Func<IAimTimer, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory)
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
            _aimTimerItemFactory = aimTimerItemFactory;
        }

        public void Init()
        {
            _aimTimerNotificationService.Stop();
            AimTimerListItemViewModels.CollectionChanged -= AimTimerItemViewModels_CollectionChanged;
            _aimTimerNotificationService.OnStatusChanged -= AimTimerNotificationService_OnStatusChanged;
            _messagingCenter.Unsubscribe<IAimTimerItem>(this, MessagingCenterMessages.AimTimerUpdated);

            AimTimerListItemViewModels.Clear();
            foreach (var aimTimerItem in _aimTimerService.GetActiveAimTimers())
            {
                AimTimerListItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimerItem));
            }

            _aimTimerNotificationService.SetItemsToFollow(AimTimerListItemViewModels.Select(i => i.GetAimTimerItem()).ToList());
            AimTimerListItemViewModels.CollectionChanged += AimTimerItemViewModels_CollectionChanged;
            _aimTimerNotificationService.OnStatusChanged += AimTimerNotificationService_OnStatusChanged;
            _aimTimerNotificationService.Start();
            _messagingCenter.Subscribe<IAimTimerItem>(this, MessagingCenterMessages.AimTimerUpdated, OnItemUpdated);

            OnPropertyChanged(nameof(Title));
        }

        private void OnItemUpdated(IAimTimerItem aimTimerItem)
        {
            var result = AimTimerListItemViewModels.FirstOrDefault(i => i.GetAimTimerItem() == aimTimerItem);
            if (result == null)
            {
                _aimTimerNotificationService.Stop();
                result = _aimTimerItemViewModelFactory.Create(aimTimerItem);
                AimTimerListItemViewModels.Add(result);
                _aimTimerNotificationService.Start();
            }
            result.Refresh();
        }

        private void AimTimerItemViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _aimTimerNotificationService.SetItemsToFollow(AimTimerListItemViewModels.Select(i => i.GetAimTimerItem()).ToList());
        }

        private void AimTimerNotificationService_OnStatusChanged(object sender, AimTimersEventArgs e)
        {
            foreach (var aimTimerItemViewModel in AimTimerListItemViewModels)
            {
                if (e.AimTimerItems.Any(i => i == aimTimerItemViewModel.GetAimTimerItem()))
                {
                    aimTimerItemViewModel.RefreshTimeLeft();
                }
            }
        }

        private async Task NavigateAimTimerView(IAimTimerItem aimTimerItem)
        {
            var aimTimerViewModel = _aimTimerViewModelFactory.Create(aimTimerItem);
            var aimTimerView = _viewFactory.CreatePopupPage(aimTimerViewModel);
            await _navigation.PushPopupAsync(aimTimerView);
        }
    }
}