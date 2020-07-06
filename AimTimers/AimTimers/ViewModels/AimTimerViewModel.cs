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

        private IAimTimerItem _aimTimerItem;

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
            if (_aimTimerItem.AimTimer.AimTimerModel.Title != Title || _aimTimerItem.AimTimerItemModel.Ticks != Time.Ticks)
            {
                _aimTimerItem.AimTimer.AimTimerModel.Title = Title;
                _aimTimerItem.AimTimerItemModel.Ticks = Time.Ticks;
                _aimTimerItem.AimTimer.AimTimerModel.Ticks = Time.Ticks;
                _aimTimerService.AddAimTimer(_aimTimerItem);
                _messagingCenter.Send(_aimTimerItem, MessagingCenterMessages.AimTimerUpdated);
            }
            await _navigation.PopPopupAsync();
        }

        #endregion

        public void Setup(IAimTimerItem aimTimerItem)
        {
            _aimTimerItem = aimTimerItem;

            Title = _aimTimerItem.AimTimer.AimTimerModel.Title;
            Time = new TimeSpan(_aimTimerItem.AimTimer.AimTimerModel.Ticks ?? 0);
        }
    }
}
