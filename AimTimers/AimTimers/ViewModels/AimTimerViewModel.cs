using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.ViewModelFactories;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IAimTimerService _aimTimerService;
        Func<AimTimerModel, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;
        private readonly IAimTimerItemListItemViewModelFactory _aimTimerItemListItemViewModelFactory;

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

        public ObservableCollection<IAimTimerItemListItemViewModel> AimTimerItems { get; set; }

        #endregion

        public AimTimerViewModel(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IAimTimerService aimTimerService,
            Func<AimTimerModel, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory,
            IAimTimerItemListItemViewModelFactory aimTimerItemListItemViewModelFactory)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
            _navigation = navigation;
            _aimTimerService = aimTimerService;
            _aimTimerItemFactory = aimTimerItemFactory;
            _aimTimerItemListItemViewModelFactory = aimTimerItemListItemViewModelFactory;
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
            _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
            await _navigation.PopAsync();
        }

        #endregion

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;

            _aimTimerNotificationService.SetItemsToFollow(new List<IAimTimer> { aimTimer });
            _aimTimerNotificationService.Start();
            _aimTimerNotificationService.OnStatusChanged += _aimTimerNotificationService_OnStatusChanged;

            AimTimerItems = new ObservableCollection<IAimTimerItemListItemViewModel>();
            foreach (var item in _aimTimer.AimTimerModel.AimTimerItemModels.OrderByDescending(i => i.EndOfActivityPeriod).ToList())
            {
                var aimTimerItem = _aimTimerItemFactory.Invoke(aimTimer.AimTimerModel, item);
                var itemToAdd = _aimTimerItemListItemViewModelFactory.Create(_aimTimer, aimTimerItem);
                AimTimerItems.Add(itemToAdd);
            }
        }

        private void _aimTimerNotificationService_OnStatusChanged(object sender, AimTimersEventArgs e)
        {
            foreach(var i in AimTimerItems)
            {
                i.RefreshTimeLeft();
            }
        }
    }
}
