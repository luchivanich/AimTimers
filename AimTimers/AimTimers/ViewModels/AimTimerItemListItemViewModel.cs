using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModelFactories;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemListItemViewModel : ObservableCollection<IAimTimerIntervalListItemViewModel>, IAimTimerItemListItemViewModel, INotifyPropertyChanged
    {
        private readonly IAimTimerService _aimTimerService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;

        private IAimTimer _aimTimer;
        private IAimTimerItem _aimTimerItem;

        public string DateRunning => _aimTimerItem.AimTimerItemModel.StartOfActivityPeriod.Date.ToShortDateString();

        public bool IsCurrent => _dateTimeProvider.GetNow().Date == _aimTimerItem.AimTimerItemModel.StartOfActivityPeriod.Date;

        public string TimeLeft => _aimTimerItem.GetTimeLeft().ToString();

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

        public ObservableCollection<IAimTimerIntervalListItemViewModel> AimTimerIntervals => this;

        public AimTimerItemListItemViewModel(
            IAimTimerService aimTimerService,
            IDateTimeProvider dateTimeProvider,
            IAimTimerIntervalListItemViewModelFactory aimTimerIntervalListItemViewModelFactory)
        {
            _aimTimerService = aimTimerService;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerIntervalListItemViewModelFactory = aimTimerIntervalListItemViewModelFactory;
        }

        public void Setup(IAimTimer aimTimer, IAimTimerItem aimTimerItem)
        {
            _aimTimer = aimTimer;
            _aimTimerItem = aimTimerItem;
            LoadIntervals();
        }

        public void RefreshTimeLeft()
        {
            if (IsCurrent)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TimeLeft)));
                // OnPropertyChanged(nameof(TimeLeft));
            }
        }

        private void LoadIntervals()
        {
            this.Clear();

            if (!_isExpanded)
            {
                return;
            }

            foreach (var interval in _aimTimerItem.AimTimerItemModel.AimTimerIntervals)
            {
                //this.Add(_aimTimerIntervalListItemViewModelFactory.Create(interval));
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return new Command(() => ExecutePauseCommand());
            }
        }

        private void ExecutePauseCommand()
        {
            _aimTimer.Stop();
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
            LoadIntervals();
        }

        public ICommand PlayCommand
        {
            get
            {
                return new Command(() => ExecutePlayCommand());
            }
        }

        private void ExecutePlayCommand()
        {
            _aimTimer.Start();
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
            LoadIntervals();
        }

        public ICommand ToggleHistoryDetailsCommand
        {
            get
            {
                return new Command(() => ExecuteToggleHistoryDetailsCommand());
            }
        }

        private void ExecuteToggleHistoryDetailsCommand()
        {
            IsExpanded = !IsExpanded;
        }
    }
}
