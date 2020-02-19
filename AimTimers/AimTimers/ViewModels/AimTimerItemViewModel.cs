using System;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModel : BaseViewModel, IAimTimerItemViewModel
    {
        private readonly IAimTimerService _aimTimerService;

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

        public string TimeLeft => _aimTimer.GetTimeLeft().ToString();

        public AimTimerItemViewModel(IAimTimerService aimTimerService)
        {
            _aimTimerService = aimTimerService;
        }

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;
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
