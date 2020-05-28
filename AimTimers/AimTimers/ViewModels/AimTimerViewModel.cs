using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IMessagingCenter _messagingCenter;
        private readonly IAimTimerService _aimTimerService;

        private string _originalTitle;
        private TimeSpan _originalTime;

        private IAimTimer _aimTimer;

        #region Properties

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

        public TimeSpan Time
        {
            get => new TimeSpan(_aimTimer.AimTimerModel.Ticks ?? 0);
            set
            {
                _aimTimer.AimTimerModel.Ticks = value.Ticks;
                OnPropertyChanged();
            }
        }

        #endregion

        public AimTimerViewModel(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IMessagingCenter messagingCenter,
            IAimTimerService aimTimerService)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
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
            if (_originalTitle != Title || _originalTime != Time)
            {
                _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
                _originalTime = Time;
                _originalTitle = Title;
            }
            _messagingCenter.Send("tmp", "AimTimerUpdated");
            await _navigation.PopPopupAsync();
        }

        #endregion

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;

            _originalTime = Time;
            _originalTitle = Title;

            _aimTimerNotificationService.SetItemsToFollow(new List<IAimTimer> { aimTimer });
            _aimTimerNotificationService.Start();
        }
    }
}
