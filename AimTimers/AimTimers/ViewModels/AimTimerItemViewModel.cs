using System;
using System.Windows.Input;
using AimTimers.Models;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModel : BaseViewModel, IAimTimerItemViewModel
    {
        private AimTimerItem _aimTimerItem;

        public string Title
        {
            get => _aimTimerItem.AimTimer.Title;
            set
            {
                _aimTimerItem.AimTimer.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _aimTimerItem.AimTimer.Description;
            set
            {
                _aimTimerItem.AimTimer.Description = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EndOfActivityPeriod
        {
            get => _aimTimerItem.EndOfActivityPeriod;
            set
            {
                _aimTimerItem.EndOfActivityPeriod = value;
                OnPropertyChanged();
            }
        }

        public string TimeLeft => _aimTimerItem.TimeLeft.ToString();

        public void Setup(AimTimerItem aimTimerItem)
        {
            _aimTimerItem = aimTimerItem;
        }

        public AimTimer GetAimTimer()
        {
            return _aimTimerItem.AimTimer;
        }

        public void RefreshTimeLeft()
        {
            OnPropertyChanged(nameof(TimeLeft));
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
            _aimTimerItem.Pause();
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
            _aimTimerItem.Start();
        }
    }
}
