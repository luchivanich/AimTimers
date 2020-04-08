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

        public AimTimerStatus Status => _aimTimer.GetAimTimerStatus();

        public AimTimerRunningStatus RunningStatus => _aimTimer.GetAimTimerRunningStatus();

        public TimeSpan Time => new TimeSpan(_aimTimer.AimTimerModel.Ticks ?? default);

        public TimeSpan TimeLeft => _aimTimer.TimeLeft;

        public TimeSpan TimePassed => Time - new TimeSpan(TimeLeft.Hours, TimeLeft.Minutes, TimeLeft.Seconds);

        public string EndOfActivityPeriod => _aimTimer.GetCurrentAimTimerItem()?.AimTimerItemModel.EndOfActivityPeriod.ToLongTimeString() ?? string.Empty;

        #region Commands

        public ICommand PlayPauseItemCommand
        {
            get
            {
                return new Command(() => ExecutePlayPauseItemCommand());
            }
        }

        private void ExecutePlayPauseItemCommand()
        {
            if (_aimTimer.GetAimTimerRunningStatus() == AimTimerRunningStatus.InProgress)
            {
                _aimTimer.Stop();
            }
            else
            {
                _aimTimer.Start();
                
            }
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
            OnPropertyChanged(nameof(RunningStatus));
            OnPropertyChanged(nameof(Status));
        }

        #endregion

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
            //if (RunningStatus == AimTimerRunningStatus.InProgress)
            //{
                _aimTimer.RefreshTimeLeft();
                OnPropertyChanged(nameof(TimeLeft));
                OnPropertyChanged(nameof(TimePassed));
                OnPropertyChanged(nameof(Status));
            //}
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Time));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(RunningStatus));

            RefreshTimeLeft();
        }
    }
}
