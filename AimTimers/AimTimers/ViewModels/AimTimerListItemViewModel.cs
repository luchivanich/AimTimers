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

namespace AimTimers.ViewModels
{
    public class AimTimerListItemViewModel : ObservableCollection<IAimTimerIntervalListItemViewModel>, IAimTimerListItemViewModel, INotifyPropertyChanged
    {
        private readonly IAlertManager _alertManager;
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;
        private readonly Func<AimTimerIntervalModel, IAimTimerInterval> _aimTimerIntervalFactory;
        private readonly Func<IAimTimerInterval, IAimTimerIntervalViewModel> _aimTimerIntervalViewModelFactory;

        private IAimTimer _aimTimer;
        private IAimTimerItem _aimTimerItem;

        public string Title
        {
            get => _aimTimer.AimTimerModel.Title;
            set
            {
                _aimTimer.AimTimerModel.Title = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        public string Description
        {
            get => _aimTimer.AimTimerModel.Description;
            set
            {
                _aimTimer.AimTimerModel.Description = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public AimTimerStatus Status => _aimTimer.GetAimTimerStatus();

        public AimTimerRunningStatus RunningStatus => _aimTimer.GetAimTimerRunningStatus();

        public TimeSpan Time => new TimeSpan(_aimTimer.AimTimerModel.Ticks ?? default);

        public TimeSpan TimeLeft => _aimTimer.TimeLeft;

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

        #region Commands

        public ICommand PlayPauseItemCommand
        {
            get
            {
                return new Command(() => ExecutePlayPauseItemCommand());
            }
        }

        private void ExecutePlayPauseItemCommand()
        {
            if (_aimTimer.GetAimTimerRunningStatus() == AimTimerRunningStatus.InProgress)
            {
                _aimTimer.Stop();
            }
            else
            {
                _aimTimer.Start();
                
            }
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
            LoadIntervals();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(RunningStatus)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
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

        public ICommand EditItemCommand
        {
            get
            {
                return new Command<IAimTimerIntervalListItemViewModel>(async (aimTimerIntervalListItemViewModel) => await ExecuteEditItemCommand(aimTimerIntervalListItemViewModel));
            }
        }

        private async Task ExecuteEditItemCommand(IAimTimerIntervalListItemViewModel aimTimerIntervalListItemViewModel)
        {
            var aimTimerIntervalViewModel = _aimTimerIntervalViewModelFactory.Invoke(aimTimerIntervalListItemViewModel.AimTimerInterval);
            var aimTimerIntervalView = _viewFactory.CreatePage(aimTimerIntervalViewModel);
            await _navigation.PushAsync(aimTimerIntervalView);
        }


        public ICommand DeleteItemCommand
        {
            get
            {
                return new Command<IAimTimerIntervalListItemViewModel>(async (aimTimerIntervalListItemViewModel) => await ExecuteDeleteItemCommand(aimTimerIntervalListItemViewModel));
            }
        }

        private async Task ExecuteDeleteItemCommand(IAimTimerIntervalListItemViewModel aimTimerIntervalListItemViewModel)
        {
            if (await _alertManager.DisplayAlert("Warning!", "Would you like to remove the interval completely?", "Yes", "No"))
            {
                _aimTimerItem.AimTimerItemModel.AimTimerIntervals.Remove(aimTimerIntervalListItemViewModel.AimTimerInterval.AimTimerIntervalModel);
                Remove(aimTimerIntervalListItemViewModel);
                _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
                Refresh();
            }
        }

        #endregion

        public AimTimerListItemViewModel(
            IAlertManager alertManager,
            INavigation navigation,
            IAimTimerService aimTimerService,
            IViewFactory viewFactory,
            IAimTimerIntervalListItemViewModelFactory aimTimerIntervalListItemViewModelFactory,
            Func<AimTimerIntervalModel, IAimTimerInterval> aimTimerIntervalFactory,
            Func<IAimTimerInterval, IAimTimerIntervalViewModel> aimTimerIntervalViewModelFactory)
        {
            _alertManager = alertManager;
            _navigation = navigation;
            _aimTimerService = aimTimerService;
            _viewFactory = viewFactory;
            _aimTimerIntervalListItemViewModelFactory = aimTimerIntervalListItemViewModelFactory;
            _aimTimerIntervalViewModelFactory = aimTimerIntervalViewModelFactory;
            _aimTimerIntervalFactory = aimTimerIntervalFactory;
        }

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;
            _aimTimerItem = _aimTimer.GetCurrentAimTimerItem();
        }

        public IAimTimer GetAimTimer()
        {
            return _aimTimer;
        }

        public void RefreshTimeLeft()
        {
            //if (RunningStatus == AimTimerRunningStatus.InProgress)
            //{
                _aimTimer.RefreshTimeLeft();
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TimeLeft)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TimePassed)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
            //}
        }

        public void Refresh()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Title)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Time)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(RunningStatus)));

            RefreshTimeLeft();
        }

        private void LoadIntervals()
        {
            this.Clear();

            if (!_isExpanded)
            {
                return;
            }

            foreach (var intervalModel in _aimTimerItem.AimTimerItemModel.AimTimerIntervals.OrderByDescending(i => i.StartDate))
            {
                var interval = _aimTimerIntervalFactory.Invoke(intervalModel);
                Add(_aimTimerIntervalListItemViewModelFactory.Create(interval, this));
            }
        }
    }
}
