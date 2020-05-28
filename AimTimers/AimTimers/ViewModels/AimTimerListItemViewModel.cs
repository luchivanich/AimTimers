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
        private readonly IAlertManager _alertManager;
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;
        private readonly Func<IAimTimer, AimTimerIntervalModel, IAimTimerInterval> _aimTimerIntervalFactory;
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

        public AimTimerStatusFlags Status => _aimTimer.GetAimTimerStatusFlags();

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

        public bool IsExpandable => _aimTimerItem.AimTimerItemModel.AimTimerIntervals.Count > 0;

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
            if ((Status & AimTimerStatusFlags.Running) == AimTimerStatusFlags.Running)
            {
                _aimTimer.Stop();
            }
            else
            {
                _aimTimer.Start();
                
            }
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
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
            //await _navigation.PopPopupAsync();
            //var aimTimerViewModel = _aimTimerViewModelFactory.Create(_aimTimerItem.AimTimer);
            //var aimTimerView = _viewFactory.CreatePopupPage(aimTimerViewModel);
            //await _navigation.PushPopupAsync(aimTimerView);
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
                _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
                Refresh();
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpandable)));
            }
        }

        #endregion

        public AimTimerListItemViewModel(
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

                foreach(var item in this)
                {
                    item.Refresh();
                }

            //}
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

            if (!_isExpanded)
            {
                return;
            }

            foreach (var intervalModel in _aimTimerItem.AimTimerItemModel.AimTimerIntervals.OrderByDescending(i => i.StartDate))
            {
                var interval = _aimTimerIntervalFactory.Invoke(_aimTimer, intervalModel);
                Add(_aimTimerIntervalListItemViewModelFactory.Create(interval, this));
            }
        }
    }
}
