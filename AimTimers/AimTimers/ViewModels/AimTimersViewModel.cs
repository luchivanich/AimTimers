using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Services;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimersViewModel : BaseViewModel, IAimTimersViewModel
    {
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
            var aimTimerViewModel = _aimTimerViewModelFactory.CreateNew();
            await _viewFactory.NavigatePageAsync(aimTimerViewModel);
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
            var aimTimerViewModel = _aimTimerViewModelFactory.Create(aimTimerItemViewModel.GetAimTimer());
            await _viewFactory.NavigatePageAsync(aimTimerViewModel);
        }

        #endregion

        public AimTimersViewModel(IViewFactory viewFactory, IAimTimerService aimTimerService, IAimTimerItemViewModelFactory aimTimerItemViewModelFactory, IAimTimerViewModelFactory aimTimerViewModelFactory)
        {
            _viewFactory = viewFactory;
            _aimTimerService = aimTimerService;
            _aimTimerItemViewModelFactory = aimTimerItemViewModelFactory;
            _aimTimerViewModelFactory = aimTimerViewModelFactory;

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;

            _timer.Enabled = true;

            LoadData();
        }

        private async void LoadData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                AimTimerItemViewModels.Clear();
                var items = await _aimTimerService.GetActiveAimTimerItems();
                foreach (var item in items)
                {
                    AimTimerItemViewModels.Add(_aimTimerItemViewModelFactory.Create(item));
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