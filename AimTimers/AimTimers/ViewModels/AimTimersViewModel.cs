using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.ViewModelFactories;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimersViewModel : BaseViewModel, IAimTimersViewModel
    {
        private readonly INavigation _navigation;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerService _aimTimerService;
        private readonly IAimTimerItemViewModelFactory _aimTimerItemViewModelFactory;
        private readonly IAimTimerViewModelFactory _aimTimerViewModelFactory;

        private System.Timers.Timer _timer;

        public ObservableCollection<IAimTimerItemViewModel> AimTimerItemViewModels { get; set; } = new ObservableCollection<IAimTimerItemViewModel>();

        #region Commands

        public ICommand LoadItemsCommand
        {
            get
            {
                return new Command(() => ExecuteLoadItemsCommand());
            }
        }

        private void ExecuteLoadItemsCommand()
        {
            LoadData();
        }

        public ICommand AddItemCommand
        {
            get
            {
                return new Command(async () => await ExecuteAddItemCommand());
            }
        }

        private async Task ExecuteAddItemCommand()
        {
            await NavigateAimTimerView(new AimTimer());
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new Command<IAimTimerItemViewModel>(async (aimTimerItemViewModel) => await ExecuteItemSelectCommand(aimTimerItemViewModel));
            }
        }

        private async Task ExecuteItemSelectCommand(IAimTimerItemViewModel aimTimerItemViewModel)
        {
            await NavigateAimTimerView(aimTimerItemViewModel.GetAimTimer());
        }

        private async Task NavigateAimTimerView(AimTimer aimTimer)
        {
            var aimTimerViewModel = _aimTimerViewModelFactory.Create(aimTimer);
            var aimTimerView = _viewFactory.CreatePage(aimTimerViewModel);
            await _navigation.PushAsync(aimTimerView);
        }

        #endregion

        public AimTimersViewModel(
            INavigation navigation,
            IViewFactory viewFactory,
            IAimTimerService aimTimerService,
            IAimTimerItemViewModelFactory aimTimerItemViewModelFactory,
            IAimTimerViewModelFactory aimTimerViewModelFactory)
        {
            _navigation = navigation;
            _viewFactory = viewFactory;
            _aimTimerService = aimTimerService;
            _aimTimerItemViewModelFactory = aimTimerItemViewModelFactory;
            _aimTimerViewModelFactory = aimTimerViewModelFactory;
        }

        public void Init()
        {
            LoadData();

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;

            _timer.Enabled = true;
        }

        private void LoadData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var today = DateTime.Today;
                AimTimerItemViewModels.Clear();
                var aimTimers = _aimTimerService.GetActiveAimTimers();
                foreach (var aimTimer in aimTimers)
                {
                    var aimTimerItem = aimTimer.GetAimTimerByDate(today);
                    AimTimerItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimer, aimTimerItem));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var i in AimTimerItemViewModels)
            {
                i.RefreshTimeLeft();
            }
        }
    }
}