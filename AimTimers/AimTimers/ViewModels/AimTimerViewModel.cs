using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
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

        public AimTimerViewModel(INavigation navigation, IAimTimerService aimTimerService)
        {
            _navigation = navigation;
            _aimTimerService = aimTimerService;
        }

        #region Commands

        public ICommand UpdateItemCommand
        {
            get
            {
                return new Command(async () => await ExecuteUpdateItemCommand());
            }
        }

        private async Task ExecuteUpdateItemCommand()
        {
            //if (_aimTimerService.GetActiveAimTimerItems().All(i => i.AimTimer != _aimTimer))
            //{
            //    _aimTimerService.AddAimTimer(_aimTimer);
            //}
            await _navigation.PopAsync();
        }

        #endregion

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;
        }
    }
}
