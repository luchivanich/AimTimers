using System;
using System.Linq;
using System.Windows.Input;
using AimTimers.Models;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModel : BaseViewModel, IAimTimerItemViewModel
    {
        private readonly IAimTimerService _aimTimerService;

        private AimTimerItem _aimTimerItem;
        private AimTimer _aimTimer;

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

        public string TimeLeft => GetTimeLeft().ToString();

        public AimTimerItemViewModel(IAimTimerService aimTimerService)
        {
            _aimTimerService = aimTimerService;
        }

        public TimeSpan GetTimeLeft()
        {
            _aimTimerItem.Refresh();
            return new TimeSpan(_aimTimer.Ticks ?? 0) - new TimeSpan(_aimTimerItem?.AimTimerIntervals?.Sum(i => (i.EndDate ?? DateTime.Now).Ticks - i.StartDate.Ticks) ?? 0);
        }

        public void Setup(AimTimer aimTimer, AimTimerItem aimTimerItem)
        {
            _aimTimer = aimTimer;
            _aimTimerItem = aimTimerItem;
        }

        public AimTimer GetAimTimer()
        {
            return _aimTimer;
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
            _aimTimerService.Stop(_aimTimer);
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
            _aimTimerService.Start(_aimTimer);
        }
    }
}
