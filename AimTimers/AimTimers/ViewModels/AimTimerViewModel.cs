using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Models;
using AimTimers.Services;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
        private AimTimer _aimTimer;

        #region Properties

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

        public TimeSpan? Time
        {
            get => _aimTimer.Time;
            set
            {
                _aimTimer.Time = value;
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
            if (_aimTimerService.GetActiveAimTimerItems().All(i => i.AimTimer != _aimTimer))
            {
                _aimTimerService.AddAimTimer(_aimTimer);
            }
            await _navigation.PopAsync();
        }

        #endregion

        public void Setup(AimTimer aimTimer)
        {
            _aimTimer = aimTimer;
        }
    }
}
