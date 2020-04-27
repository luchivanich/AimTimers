using System;
using System.Windows.Input;
using AimTimers.Bl;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerIntervalListItemViewModel : IAimTimerIntervalListItemViewModel
    {
        public IAimTimerInterval AimTimerInterval { get; set; }

        public IAimTimerListItemViewModel Parent { get; set; }

        public DateTime StartDate => AimTimerInterval.AimTimerIntervalModel.StartDate;
        public DateTime? EndDate => AimTimerInterval.AimTimerIntervalModel.EndDate;

        public string StartDateString => StartDate.ToLongTimeString();

        public string EndDateString => EndDate?.ToLongTimeString() ?? string.Empty;

        public string Duration => EndDate.HasValue ? (EndDate.Value - StartDate).ToString(@"hh\:mm\:ss") : string.Empty;

        public ICommand EditItemCommand
        {
            get
            {
                return new Command(() => ExecuteEditItemCommand());
            }
        }

        private void ExecuteEditItemCommand()
        {
            Parent.EditItemCommand.Execute(this);
        }


        public ICommand DeleteItemCommand
        {
            get
            {
                return new Command(() => ExecuteDeleteItemCommand());
            }
        }

        private void ExecuteDeleteItemCommand()
        {
            Parent.DeleteItemCommand.Execute(this);
        }
    }
}
