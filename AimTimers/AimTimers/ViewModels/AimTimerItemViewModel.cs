using System;
using System.Linq;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModel : BaseViewModel, IAimTimerItemViewModel
    {
        private readonly IAimTimerService _aimTimerService;

        private IAimTimerItem _aimTimerItem;
        private IAimTimer _aimTimer;

        public string Title
        {
            get => _aimTimer.AimTimerModel.Title;
            set
            {
                _aimTimer.AimTimerModel.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _aimTimer.AimTimerModel.Description;
            set
            {
                _aimTimer.AimTimerModel.Description = value;
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
            return new TimeSpan(_aimTimer.AimTimerModel.Ticks ?? 0) - new TimeSpan(_aimTimerItem.AimTimerItemModel.AimTimerIntervals?.Sum(i => (i.EndDate ?? DateTime.Now).Ticks - i.StartDate.Ticks) ?? 0);
        }

        public void Setup(IAimTimer aimTimer, IAimTimerItem aimTimerItem)
        {
            _aimTimer = aimTimer;
            _aimTimerItem = aimTimerItem;
        }

        public IAimTimer GetAimTimer()
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
            _aimTimer.Stop();
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
            _aimTimer.Start();
        }
    }
}
