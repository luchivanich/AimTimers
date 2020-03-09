using System.Collections.ObjectModel;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModelFactories;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemListItemViewModel : BaseViewModel, IAimTimerItemListItemViewModel
    {
        private readonly IAimTimerService _aimTimerService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;

        private IAimTimer _aimTimer;
        private IAimTimerItem _aimTimerItem;

        public string DateRunning => _aimTimerItem.AimTimerItemModel.StartOfActivityPeriod.Date.ToShortDateString();

        public bool IsCurrent => _dateTimeProvider.GetNow().Date == _aimTimerItem.AimTimerItemModel.StartOfActivityPeriod.Date;

        public string TimeLeft => _aimTimerItem.GetTimeLeft().ToString();

        public ObservableCollection<IAimTimerIntervalListItemViewModel> AimTimerIntervals { get; set; }

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
                OnPropertyChanged(nameof(TimeLeft));
            }
        }

        private void LoadIntervals()
        {
            AimTimerIntervals = new ObservableCollection<IAimTimerIntervalListItemViewModel>();
            foreach (var interval in _aimTimerItem.AimTimerItemModel.AimTimerIntervals)
            {
                AimTimerIntervals.Add(_aimTimerIntervalListItemViewModelFactory.Create(interval));
            }
            OnPropertyChanged(nameof(AimTimerIntervals));
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
    }
}
