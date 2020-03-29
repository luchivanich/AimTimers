using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.ViewModelFactories;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimersViewModel : BaseViewModel, IAimTimersViewModel
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimerService _aimTimerService;
        private readonly IAimTimerItemViewModelFactory _aimTimerItemViewModelFactory;
        private readonly IAimTimerViewModelFactory _aimTimerViewModelFactory;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;

        public ObservableCollection<IAimTimerListItemViewModel> AimTimerItemViewModels { get; set; } = new ObservableCollection<IAimTimerListItemViewModel>();

        #region Commands

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() => ExecuteRefreshCommandCommand());
            }
        }

        private void ExecuteRefreshCommandCommand()
        {
            LoadData();
        }

        public ICommand FreezeCommand
        {
            get
            {
                return new Command(() => ExecuteFreezeCommandCommand());
            }
        }

        private void ExecuteFreezeCommandCommand()
        {
            _aimTimerNotificationService.OnStatusChanged -= _aimTimerNotificationService_OnStatusChanged;
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
            await NavigateAimTimerView(_aimTimerFactory.Invoke(new AimTimerModel()));
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new Command<IAimTimerListItemViewModel>(async (aimTimerItemViewModel) => await ExecuteItemSelectCommand(aimTimerItemViewModel));
            }
        }

        private async Task ExecuteItemSelectCommand(IAimTimerListItemViewModel aimTimerItemViewModel)
        {
            await NavigateAimTimerView(aimTimerItemViewModel.GetAimTimer());
        }

        private async Task NavigateAimTimerView(IAimTimer aimTimer)
        {
            var aimTimerViewModel = _aimTimerViewModelFactory.Create(aimTimer);
            var aimTimerView = _viewFactory.CreatePage(aimTimerViewModel);
            await _navigation.PushAsync(aimTimerView);
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Command<AimTimerListItemViewModel>((aimTimerListItemViewModel) => ExecuteDeleteCommand(aimTimerListItemViewModel));
            }
        }

        private void ExecuteDeleteCommand(AimTimerListItemViewModel aimTimerListItemViewModel)
        {
            _aimTimerService.DeleteAimTimer(aimTimerListItemViewModel.GetAimTimer()?.AimTimerModel.Id);
            LoadData();
        }

        #endregion

        public AimTimersViewModel(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IViewFactory viewFactory,
            IAimTimerService aimTimerService,
            IAimTimerItemViewModelFactory aimTimerItemViewModelFactory,
            IAimTimerViewModelFactory aimTimerViewModelFactory,
            Func<AimTimerModel, IAimTimer> aimTimerFactory)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
            _navigation = navigation;
            _viewFactory = viewFactory;
            _aimTimerService = aimTimerService;
            _aimTimerItemViewModelFactory = aimTimerItemViewModelFactory;
            _aimTimerViewModelFactory = aimTimerViewModelFactory;
            _aimTimerFactory = aimTimerFactory;
        }

        private void LoadData()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            AimTimerItemViewModels.Clear();
            foreach (var aimTimerModel in _aimTimerService.GetActiveAimTimers())
            {
                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                AimTimerItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimer));
            }

            _aimTimerNotificationService.SetItemsToFollow(AimTimerItemViewModels.Select(i => i.GetAimTimer()).ToList());
            _aimTimerNotificationService.Start();
            _aimTimerNotificationService.OnStatusChanged += _aimTimerNotificationService_OnStatusChanged;

            AimTimerItemViewModels.CollectionChanged += AimTimerItemViewModels_CollectionChanged;

            IsBusy = false;
        }

        private void AimTimerItemViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _aimTimerNotificationService.SetItemsToFollow(AimTimerItemViewModels.Select(i => i.GetAimTimer()).ToList());
        }

        private void _aimTimerNotificationService_OnStatusChanged(object sender, AimTimersEventArgs e)
        {
            foreach (var aimTimerItemViewModel in AimTimerItemViewModels)
            {
                if (e.AimTimers.Any(i => i == aimTimerItemViewModel.GetAimTimer()))
                {
                    aimTimerItemViewModel.RefreshTimeLeft();
                }
            }
        }
    }
}