using System;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerIntervalViewModel : BaseViewModel, IAimTimerIntervalViewModel
    {
        private readonly IDateTimeProvider _dateTimeProvider;
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

        public AimTimerIntervalViewModel(IDateTimeProvider dateTimeProvider, IAimTimerService aimTimerService)
        {
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
    }
}
