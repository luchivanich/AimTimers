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
        private readonly IAimTimerService _aimTimerService;

        private IAimTimerInterval _aimTimerInterval;

        private TimeSpan _originalStartTime;
        private TimeSpan _originalEndTime;

        public TimeSpan StartTime
        {
            get => _aimTimerInterval.AimTimerIntervalModel.StartDate.TimeOfDay;
            set
            {
                if (value > EndTime)
                {
                    value = EndTime;
                }
                var now = _dateTimeProvider.GetNow();
                _aimTimerInterval.AimTimerIntervalModel.StartDate = new DateTime(now.Year, now.Month, now.Day, value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public TimeSpan EndTime
        {
            get => _aimTimerInterval.AimTimerIntervalModel.EndDate?.TimeOfDay ?? default;
            set
            {
                if (value < StartTime)
                {
                    value = StartTime;
                }
                var now = _dateTimeProvider.GetNow();
                _aimTimerInterval.AimTimerIntervalModel.EndDate = new DateTime(now.Year, now.Month, now.Day, value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Duration));
            }
        }

        public TimeSpan Duration => EndTime - StartTime;

        public AimTimerIntervalViewModel(IDateTimeProvider dateTimeProvider, INavigation navigation, IAimTimerService aimTimerService)
        {
            _navigation = navigation;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerService = aimTimerService;
        }

        internal void Setup(IAimTimerInterval aimTimerInterval)
        {
            _aimTimerInterval = aimTimerInterval;
            _originalStartTime = StartTime;
            _originalEndTime = EndTime;
        }

        public ICommand UpdateItemCommand
        {
            get
            {
                return new Command(() => ExecuteUpdateItemCommand());
            }
        }

        private void ExecuteUpdateItemCommand()
        {
            if (_originalStartTime != StartTime || _originalEndTime != EndTime)
            {
                _aimTimerService.AddAimTimer(_aimTimerInterval.AimTimer.AimTimerModel);
                _originalStartTime = StartTime;
                _originalEndTime = EndTime;
            }
        }

        public ICommand UpdateAndCloseCommand
        {
            get
            {
                return new Command(async () => await ExecuteUpdateAndCloseCommand());
            }
        }

        private async Task ExecuteUpdateAndCloseCommand()
        {
            //if (_originalStartTime != StartTime || _originalEndTime != EndTime)
            //{
            //    _aimTimerService.AddAimTimer(_aimTimerInterval.AimTimer.AimTimerModel);
            //    _originalStartTime = StartTime;
            //    _originalEndTime = EndTime;
            //}
            await _navigation.PopPopupAsync();
        }
    }
}
