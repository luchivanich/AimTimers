using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerIntervalViewModel : BaseViewModel, IAimTimerIntervalViewModel
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly INavigation _navigation;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IAimTimerService _aimTimerService;

        private IAimTimerInterval _aimTimerInterval;

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (value > EndTime)
                {
                    value = EndTime;
                }
                _startTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (value < StartTime)
                {
                    value = StartTime;
                }
                _endTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public TimeSpan Duration => EndTime - StartTime;

        public AimTimerIntervalViewModel(
            IDateTimeProvider dateTimeProvider,
            INavigation navigation,
            IMessagingCenter messagingCenter,
            IAimTimerService aimTimerService)
        {
            _dateTimeProvider = dateTimeProvider;
            _navigation = navigation;
            _messagingCenter = messagingCenter;
            _aimTimerService = aimTimerService;
        }

        internal void Setup(IAimTimerInterval aimTimerInterval)
        {
            _aimTimerInterval = aimTimerInterval;
            EndTime = _aimTimerInterval.AimTimerIntervalModel.EndDate?.TimeOfDay ?? default;
            StartTime = _aimTimerInterval.AimTimerIntervalModel.StartDate.TimeOfDay;
        }

        public ICommand AcceptCommand
        {
            get
            {
                return new Command(async () => await ExecuteAcceptCommand());
            }
        }

        private async Task ExecuteAcceptCommand()
        {
            if (_aimTimerInterval.AimTimerIntervalModel.StartDate.TimeOfDay != StartTime ||
                (_aimTimerInterval.AimTimerIntervalModel.EndDate?.TimeOfDay ?? default) != EndTime)
            {
                var now = _dateTimeProvider.GetNow();
                _aimTimerInterval.AimTimerIntervalModel.StartDate = new DateTime(now.Year, now.Month, now.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
                _aimTimerInterval.AimTimerIntervalModel.EndDate = new DateTime(now.Year, now.Month, now.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);

                _aimTimerService.AddAimTimer(_aimTimerInterval.AimTimerItem);
                _messagingCenter.Send(_aimTimerInterval, MessagingCenterMessages.AimTimerIntervalUpdated);
            }

            await _navigation.PopPopupAsync();
        }
    }
}
