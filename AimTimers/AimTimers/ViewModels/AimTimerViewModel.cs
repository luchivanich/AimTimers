using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private readonly INavigation _navigation;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IAimTimerService _aimTimerService;

        private IAimTimer _aimTimer;

        #region Properties

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public AimTimerViewModel(
            INavigation navigation,
            IMessagingCenter messagingCenter,
            IAimTimerService aimTimerService)
        {
            _navigation = navigation;
            _messagingCenter = messagingCenter;
            _aimTimerService = aimTimerService;
        }

        #region Commands

        public ICommand AcceptCommand
        {
            get
            {
                return new Command(async () => await ExecuteAcceptCommand());
            }
        }

        private async Task ExecuteAcceptCommand()
        {
            if (_aimTimer.AimTimerModel.Title != Title || _aimTimer.AimTimerModel.Ticks != Time.Ticks)
            {
                _aimTimer.AimTimerModel.Title = Title;
                _aimTimer.AimTimerModel.Ticks = Time.Ticks;
                _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
                _messagingCenter.Send(_aimTimer, MessagingCenterMessages.AimTimerUpdated);
            }
            await _navigation.PopPopupAsync();
        }

        #endregion

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;

            Title = aimTimer.AimTimerModel.Title;
            Time = new TimeSpan(aimTimer.AimTimerModel.Ticks ?? 0);
        }
    }
}
