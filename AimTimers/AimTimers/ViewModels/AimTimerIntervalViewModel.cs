using System;
using AimTimers.Bl;

namespace AimTimers.ViewModels
{
    public class AimTimerIntervalViewModel : BaseViewModel, IAimTimerIntervalViewModel
    {
        private IAimTimerInterval _aimTimerInterval;

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        internal void Setup(IAimTimerInterval aimTimerInterval)
        {
            _aimTimerInterval = aimTimerInterval;
        }
    }
}
