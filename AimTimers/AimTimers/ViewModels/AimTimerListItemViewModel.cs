using System;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerListItemViewModel : BaseViewModel, IAimTimerListItemViewModel
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

        public bool IsInProgress => _aimTimer.GetAimTimerStatus(DateTime.Now) == AimTimerStatus.InProgress;

        public string Time => (new TimeSpan(_aimTimer.AimTimerModel.Ticks ?? default)).ToString(@"hh\:mm\:ss");

        public string TimeLeft => _aimTimer.GetTimeLeft().ToString(@"hh\:mm\:ss");

        public string TimePassed => (new TimeSpan(_aimTimer.AimTimerModel.Ticks ?? default) - _aimTimer.GetTimeLeft()).ToString(@"hh\:mm\:ss");

        public string EndOfActivityPeriod => _aimTimer.GetAimTimerItemByDate(DateTime.Now)?.AimTimerItemModel.EndOfActivityPeriod.ToLongTimeString() ?? string.Empty;

        public AimTimerListItemViewModel(IAimTimerService aimTimerService)
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
            OnPropertyChanged(nameof(TimePassed));
        }

        public ICommand PlayPauseItemCommand
        {
            get
            {
                return new Command(() => ExecutePlayPauseItemCommand());
            }
        }

        private void ExecutePlayPauseItemCommand()
        {
            if (_aimTimer.GetAimTimerStatus(DateTime.Now) == AimTimerStatus.InProgress)
            {
                _aimTimer.Stop();
            }
            else
            {
                _aimTimer.Start();
                
            }
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
            OnPropertyChanged(nameof(IsInProgress));
        }
    }
}
