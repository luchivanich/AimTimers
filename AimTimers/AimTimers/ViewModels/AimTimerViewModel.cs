using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Models;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private AimTimer _aimTimer;

        #region Properties

        public string Title
        {
            get => _aimTimer.Title;
            set
            {
                _aimTimer.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _aimTimer.Description;
            set
            {
                _aimTimer.Description = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan? Time
        {
            get => _aimTimer.Time;
            set
            {
                _aimTimer.Time = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateItemCommand
        {
            get
            {
                return new Command(async () => await ExecuteUpdateItemCommand());
            }
        }

        private async Task ExecuteUpdateItemCommand()
        {

        }

        #endregion

        public void Setup(AimTimer aimTimer)
        {
            _aimTimer = aimTimer;
        }
    }
}
