using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemListItemViewModel : BaseViewModel, IAimTimerItemListItemViewModel
    {
        private readonly IAimTimerService _aimTimerService;
        private readonly IDateTimeProvider _dateTimeProvider;

        private IAimTimer _aimTimer;
        private IAimTimerItem _aimTimerItem;

        public string DateRunning => _aimTimerItem.AimTimerItemModel.StartOfActivityPeriod.Date.ToShortDateString();

        public bool IsCurrent => _dateTimeProvider.GetNow().Date == _aimTimerItem.AimTimerItemModel.StartOfActivityPeriod.Date;

        public string TimeLeft => _aimTimerItem.GetTimeLeft().ToString();

        public AimTimerItemListItemViewModel(IAimTimerService aimTimerService, IDateTimeProvider dateTimeProvider)
        {
            _aimTimerService = aimTimerService;
            _dateTimeProvider = dateTimeProvider;
        }

        public void Setup(IAimTimer aimTimer, IAimTimerItem aimTimerItem)
        {
            _aimTimer = aimTimer;
            _aimTimerItem = aimTimerItem;
        }

        public IAimTimer GetAimTimer()
        {
            return _aimTimer;
        }

        public void RefreshTimeLeft()
        {
            if (IsCurrent)
            {
                OnPropertyChanged(nameof(TimeLeft));
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
        }
    }
}
