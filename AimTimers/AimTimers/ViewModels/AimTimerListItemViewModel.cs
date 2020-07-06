using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModelFactories;
using AimTimers.Views;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace AimTimers.ViewModels
{
    public class AimTimerListItemViewModel : ObservableCollection<IAimTimerIntervalListItemViewModel>, IAimTimerListItemViewModel, INotifyPropertyChanged
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAlertManager _alertManager;
        private readonly INavigation _navigation;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IAimTimerService _aimTimerService;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;
        private readonly Func<IAimTimerItem, AimTimerIntervalModel, IAimTimerInterval> _aimTimerIntervalFactory;
        private readonly Func<IAimTimerInterval, IAimTimerIntervalViewModel> _aimTimerIntervalViewModelFactory;

        //private IAimTimer _aimTimer;
        private IAimTimerItem _aimTimerItem;

        public string Title => _aimTimerItem.AimTimer.AimTimerModel.Title;

        public AimTimerStatusFlags Status => _aimTimerItem.GetAimTimerStatusFlags();

        public TimeSpan Time => new TimeSpan(_aimTimerItem.AimTimerItemModel.Ticks ?? default);

        public TimeSpan TimeLeft => _aimTimerItem.GetTimeLeft();

        public TimeSpan TimePassed => Time - new TimeSpan(TimeLeft.Hours, TimeLeft.Minutes, TimeLeft.Seconds);

        public string EndOfActivityPeriod => _aimTimerItem.AimTimerItemModel.EndOfActivityPeriod.ToLongTimeString() ?? string.Empty;

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpanded)));
                LoadIntervals();
            }
        }

        public bool IsExpandable => _aimTimerItem.AimTimerItemModel.AimTimerIntervals.Count > 0;

        #region Commands

        public ICommand PlayPauseItemCommand
        {
            get
            {
                return new Command(() => ExecutePlayPauseItemCommand(), () => _dateTimeProvider.IsToday);
            }
        }

        private void ExecutePlayPauseItemCommand()
        {
            if ((Status & AimTimerStatusFlags.Running) == AimTimerStatusFlags.Running)
            {
                _aimTimerItem.Stop();
            }
            else
            {
                _aimTimerItem.Start();
            }
            _aimTimerService.AddAimTimer(_aimTimerItem);
            LoadIntervals();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpandable)));
        }

        public ICommand ToggleExpandCommand
        {
            get
            {
                return new Command(() => ExecuteToggleExpandCommand());
            }
        }

        private void ExecuteToggleExpandCommand()
        {
            IsExpanded = !IsExpanded;
        }

        public ICommand EditIntervalCommand
        {
            get
            {
                return new Command<IAimTimerIntervalListItemViewModel>(
                    async (aimTimerIntervalListItemViewModel) => await ExecuteEditIntervalCommand(aimTimerIntervalListItemViewModel));
            }
        }

        private async Task ExecuteEditIntervalCommand(IAimTimerIntervalListItemViewModel aimTimerIntervalListItemViewModel)
        {
            var aimTimerIntervalViewModel = _aimTimerIntervalViewModelFactory.Invoke(aimTimerIntervalListItemViewModel.AimTimerInterval);
            var aimTimerIntervalView = _viewFactory.CreatePopupPage(aimTimerIntervalViewModel);
            await _navigation.PushPopupAsync(aimTimerIntervalView);
        }

        public ICommand AddIntervalItemCommand
        {
            get
            {
                return new Command(async () => await ExecuteAddIntervalItemCommand());
            }
        }

        private async Task ExecuteAddIntervalItemCommand()
        {
            var intervalModel = new AimTimerIntervalModel { StartDate = _dateTimeProvider.GetNow(), EndDate = _dateTimeProvider.GetNow() };
            var interval = _aimTimerIntervalFactory.Invoke(_aimTimerItem, intervalModel);
            var aimTimerIntervalListItemViewModel = _aimTimerIntervalListItemViewModelFactory.Create(interval, this);

            var aimTimerIntervalViewModel = _aimTimerIntervalViewModelFactory.Invoke(aimTimerIntervalListItemViewModel.AimTimerInterval);
            var aimTimerIntervalView = _viewFactory.CreatePopupPage(aimTimerIntervalViewModel);
            await _navigation.PushPopupAsync(aimTimerIntervalView);
        }

        public ICommand DeleteIntervalCommand
        {
            get
            {
                return new Command<IAimTimerIntervalListItemViewModel>(
                    async (aimTimerIntervalListItemViewModel) => await ExecuteDeleteIntervalCommand(aimTimerIntervalListItemViewModel));
            }
        }

        private async Task ExecuteDeleteIntervalCommand(IAimTimerIntervalListItemViewModel aimTimerIntervalListItemViewModel)
        {
            if (await _alertManager.DisplayAlert("Warning!", "Would you like to remove the interval completely?", "Yes", "No"))
            {
                _aimTimerItem.AimTimerItemModel.AimTimerIntervals.Remove(aimTimerIntervalListItemViewModel.AimTimerInterval.AimTimerIntervalModel);
                Remove(aimTimerIntervalListItemViewModel);
                _aimTimerService.AddAimTimer(_aimTimerItem);
                Refresh();
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpandable)));
                if (!IsExpandable)
                {
                    IsExpanded = false;
                }
            }
        }

        //public ICommand MinusDayCommand
        //{
        //    get
        //    {
        //        return new Command(() => ExecuteMinusDayCommand());
        //    }
        //}

        //private void ExecuteMinusDayCommand()
        //{

        //}

        #endregion

        public AimTimerListItemViewModel(
            IDateTimeProvider dateTimeProvider,
            IAlertManager alertManager,
            INavigation navigation,
            IMessagingCenter messagingCenter,
            IAimTimerService aimTimerService,
            IViewFactory viewFactory,
            IAimTimerIntervalListItemViewModelFactory aimTimerIntervalListItemViewModelFactory,
            Func<IAimTimerItem, AimTimerIntervalModel, IAimTimerInterval> aimTimerIntervalFactory,
            Func<IAimTimerInterval, IAimTimerIntervalViewModel> aimTimerIntervalViewModelFactory)
        {
            _dateTimeProvider = dateTimeProvider;
            _alertManager = alertManager;
            _navigation = navigation;
            _messagingCenter = messagingCenter;
            _aimTimerService = aimTimerService;
            _viewFactory = viewFactory;
            _aimTimerIntervalListItemViewModelFactory = aimTimerIntervalListItemViewModelFactory;
            _aimTimerIntervalViewModelFactory = aimTimerIntervalViewModelFactory;
            _aimTimerIntervalFactory = aimTimerIntervalFactory;
        }

        public void Setup(IAimTimerItem aimTimerItem)
        {
            _aimTimerItem = aimTimerItem;
            _messagingCenter.Subscribe<IAimTimerInterval>(this, MessagingCenterMessages.AimTimerIntervalUpdated, OnIntervalUpdated);
        }

        public IAimTimerItem GetAimTimerItem()
        {
            return _aimTimerItem;
        }

        public void RefreshTimeLeft()
        {
            //_aimTimer.RefreshTimeLeft();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TimeLeft)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TimePassed)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));

            foreach(var item in this)
            {
                item.Refresh();
            }
        }

        public void Refresh()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Title)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Time)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));

            RefreshTimeLeft();
        }

        private void LoadIntervals()
        {
            this.Clear();

            if (!IsExpanded)
            {
                return;
            }

            foreach (var intervalModel in _aimTimerItem.AimTimerItemModel.AimTimerIntervals.OrderByDescending(i => i.StartDate))
            {
                var interval = _aimTimerIntervalFactory.Invoke(_aimTimerItem, intervalModel);
                Add(_aimTimerIntervalListItemViewModelFactory.Create(interval, this));
            }
        }

        private void OnIntervalUpdated(IAimTimerInterval aimTimerInterval)
        {
            if (aimTimerInterval.AimTimerItem != _aimTimerItem)
            {
                return;
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpandable)));

            if (IsExpanded)
            {
                var result = this.FirstOrDefault(i => i.AimTimerInterval == aimTimerInterval);
                if (result == null)
                {
                    result = _aimTimerIntervalListItemViewModelFactory.Create(aimTimerInterval, this);
                    Add(result);
                }
                result.Refresh();
            }
            Refresh();
        }
    }
}
