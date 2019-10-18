using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Xamarin.Forms;

using AimTimers.Services;
using System.Windows.Input;
using AimTimers.Views;
using System.Threading.Tasks;

namespace AimTimers.ViewModels
{
    public class AimTimersViewModel : BaseViewModel, IAimTimersViewModel
    {
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerService _aimTimerService;
        private readonly IAimTimerItemViewModelFactory _aimTimerItemViewModelFactory;

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
            var aimTimerItemViewModel = _aimTimerItemViewModelFactory.CreateNew();
            await _viewFactory.NavigatePageAsync(aimTimerItemViewModel);

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
            await _viewFactory.NavigatePageAsync(aimTimerItemViewModel);
        }

        #endregion

        public AimTimersViewModel(IViewFactory viewFactory, IAimTimerService aimTimerService, IAimTimerItemViewModelFactory aimTimerItemViewModelFactory)
        {
            _viewFactory = viewFactory;
            _aimTimerService = aimTimerService;
            _aimTimerItemViewModelFactory = aimTimerItemViewModelFactory;

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimedEvent;

            _timer.Enabled = true;


            //MessagingCenter.Subscribe<NewItemPage, AimTimer>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as AimTimer;
            //    var aimTimerViewModel = DependencyService.Resolve<AimTimerItemViewModel>();
            //    aimTimerViewModel.SetAimTimer(newItem);
            //    Items.Add(aimTimerViewModel);
            //    await DataStore.AddItemAsync(newItem);
            //});
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