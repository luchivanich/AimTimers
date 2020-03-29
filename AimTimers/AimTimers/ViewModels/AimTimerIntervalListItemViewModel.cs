using System;

namespace AimTimers.ViewModels
{
    public class AimTimerIntervalListItemViewModel : IAimTimerIntervalListItemViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string StartDateString => StartDate.ToLongTimeString();

        public string EndDateString => EndDate?.ToLongTimeString() ?? string.Empty;

        public string Duration => EndDate.HasValue ? (EndDate.Value - StartDate).ToString(@"hh\:mm\:ss") : string.Empty;
    }
}
