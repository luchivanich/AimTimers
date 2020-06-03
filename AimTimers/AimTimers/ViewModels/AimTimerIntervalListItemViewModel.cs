using System;
using AimTimers.Bl;

namespace AimTimers.ViewModels
{
    public class AimTimerIntervalListItemViewModel : BaseViewModel, IAimTimerIntervalListItemViewModel
    {
        public IAimTimerInterval AimTimerInterval { get; set; }

        public IAimTimerListItemViewModel Parent { get; set; }

        public DateTime StartDate => AimTimerInterval.AimTimerIntervalModel.StartDate;
        public DateTime? EndDate => AimTimerInterval.AimTimerIntervalModel.EndDate;

        public string StartDateString => StartDate.ToLongTimeString();

        public string EndDateString => EndDate?.ToLongTimeString() ?? string.Empty;

        public string Duration => GetDuration();

        public void Refresh()
        {
            OnPropertyChanged(nameof(StartDateString));
            OnPropertyChanged(nameof(EndDateString));
            OnPropertyChanged(nameof(Duration));
        }

        private string GetDuration()
        {
            if (EndDate == null)
            {
                return string.Empty;
            }
            var endDate = new TimeSpan(EndDate.Value.Hour, EndDate.Value.Minute, EndDate.Value.Second);
            var startDate = new TimeSpan(StartDate.Hour, StartDate.Minute, StartDate.Second);
            return (endDate - startDate).ToString(@"hh\:mm\:ss");
        }
    }
}
