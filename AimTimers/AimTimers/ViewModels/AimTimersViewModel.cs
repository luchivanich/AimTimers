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
        private readonly IAimTimerListItemViewModelFactory _aimTimerItemViewModelFactory;
        private readonly IAimTimerViewModelFactory _aimTimerViewModelFactory;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;

        private bool _isLoaded;

        public ObservableCollection<IAimTimerListItemViewModel> AimTimerItemViewModels { get; set; } = new ObservableCollection<IAimTimerListItemViewModel>();

        public string Title => "Active Timers";

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
            if (!_isLoaded)
            {
                InitialLoad();
                _isLoaded = true;
            }
            else
            {
                foreach(var i in AimTimerItemViewModels)
                {
                    i.Refresh();
                }
            }
            _aimTimerNotificationService.OnStatusChanged += _aimTimerNotificationService_OnStatusChanged;
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
            var aimTimer = _aimTimerFactory.Invoke(new AimTimerModel());
            await NavigateAimTimerView(aimTimer);
            AimTimerItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimer));
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new Command<IAimTimerListItemViewModel>(async (aimTimerListItemViewModel) => await ExecuteItemSelectCommand(aimTimerListItemViewModel));
            }
        }

        private async Task ExecuteItemSelectCommand(IAimTimerListItemViewModel aimTimerListItemViewModel)
        {
            await NavigateAimTimerView(aimTimerListItemViewModel.GetAimTimer());
        }

        private async Task NavigateAimTimerView(IAimTimer aimTimer)
        {
            var aimTimerViewModel = _aimTimerViewModelFactory.Create(aimTimer);
            var aimTimerView = _viewFactory.CreatePage(aimTimerViewModel);
            await _navigation.PushAsync(aimTimerView);
        }

        public ICommand DeleteItemCommand
        {
            get
            {
                return new Command<AimTimerListItemViewModel>((aimTimerListItemViewModel) => ExecuteDeleteItemCommand(aimTimerListItemViewModel));
            }
        }

        private void ExecuteDeleteItemCommand(IAimTimerListItemViewModel aimTimerListItemViewModel)
        {
            _aimTimerService.DeleteAimTimer(aimTimerListItemViewModel.GetAimTimer()?.AimTimerModel.Id);
            AimTimerItemViewModels.Remove(aimTimerListItemViewModel);
        }

        public ICommand ToggleCancelItemCommand
        {
            get
            {
                return new Command<AimTimerListItemViewModel>((aimTimerListItemViewModel) => ExecuteToggleCancelItemCommand(aimTimerListItemViewModel));
            }
        }

        private void ExecuteToggleCancelItemCommand(IAimTimerListItemViewModel aimTimerListItemViewModel)
        {
            var aimTimer = aimTimerListItemViewModel.GetAimTimer();
            var isCanceled = aimTimer.GetCurrentAimTimerItem().AimTimerItemModel.IsCanceled;
            aimTimer.SetIsCanceled(!isCanceled);
            _aimTimerService.AddAimTimer(aimTimerListItemViewModel.GetAimTimer().AimTimerModel);
        }

        #endregion

        public AimTimersViewModel(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IViewFactory viewFactory,
            IAimTimerService aimTimerService,
            IAimTimerListItemViewModelFactory aimTimerItemViewModelFactory,
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

        private void InitialLoad()
        {
            AimTimerItemViewModels.Clear();
            foreach (var aimTimerModel in _aimTimerService.GetActiveAimTimers())
            {
                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                AimTimerItemViewModels.Add(_aimTimerItemViewModelFactory.Create(aimTimer));
            }

            _aimTimerNotificationService.SetItemsToFollow(AimTimerItemViewModels.Select(i => i.GetAimTimer()).ToList());
            _aimTimerNotificationService.Start();
            AimTimerItemViewModels.CollectionChanged += AimTimerItemViewModels_CollectionChanged;
        }

        private void AimTimerItemViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _aimTimerNotificationService.SetItemsToFollow(AimTimerItemViewModels.Select(i => i.GetAimTimer()).ToList());
        }

        private void _aimTimerNotificationService_OnStatusChanged(object sender, AimTimersEventArgs e)
        {
            Console.Write(DateTime.Now);

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