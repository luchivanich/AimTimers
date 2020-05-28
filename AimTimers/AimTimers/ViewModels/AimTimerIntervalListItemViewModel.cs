using System;
using System.Windows.Input;
using AimTimers.Bl;
using Xamarin.Forms;

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

        public string Duration => EndDate.HasValue ? (EndDate.Value - StartDate).ToString(@"hh\:mm\:ss") : string.Empty;

        public void Refresh()
        {
            OnPropertyChanged(nameof(StartDateString));
            OnPropertyChanged(nameof(EndDateString));
            OnPropertyChanged(nameof(Duration));
        }
    }
}
