using System.Windows.Input;
using AimTimers.Models;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModel : BaseViewModel, IAimTimerItemViewModel
    {
        private IAimTimerTickService _aimTimerService;

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
        public string TimeLeft => _aimTimerItem.TimeLeft.ToString();

        public AimTimerItemViewModel(IAimTimerTickService aimTimerService)
        {
            _aimTimerService = aimTimerService;
        }

        public void SetAimTimer(AimTimerItem aimTimerItem)
        {
            _aimTimerItem = aimTimerItem;
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
