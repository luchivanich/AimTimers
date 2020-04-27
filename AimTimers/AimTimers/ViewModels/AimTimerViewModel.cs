using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModelFactories;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModel : BaseViewModel, IAimTimerViewModel
    {
        private readonly IAimTimerNotificationService _aimTimerNotificationService;
        private readonly INavigation _navigation;
        private readonly IAlertManager _alertManager;
        private readonly IAimTimerService _aimTimerService;
        Func<AimTimerModel, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;
        private readonly IAimTimerItemListItemViewModelFactory _aimTimerItemListItemViewModelFactory;

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

        public ObservableCollection<IAimTimerItemListItemViewModel> AimTimerItems { get; set; }

        #endregion

        public AimTimerViewModel(
            IAimTimerNotificationService aimTimerNotificationService,
            INavigation navigation,
            IAlertManager alertManager,
            IAimTimerService aimTimerService,
            Func<AimTimerModel, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory,
            IAimTimerItemListItemViewModelFactory aimTimerItemListItemViewModelFactory)
        {
            _aimTimerNotificationService = aimTimerNotificationService;
            _navigation = navigation;
            _alertManager = alertManager;
            _aimTimerService = aimTimerService;
            _aimTimerItemFactory = aimTimerItemFactory;
            _aimTimerItemListItemViewModelFactory = aimTimerItemListItemViewModelFactory;
        }

        #region Commands

        public ICommand UpdateItemCommand
        {
            get
            {
                return new Command(() => ExecuteUpdateItemCommand());
            }
        }

        private void ExecuteUpdateItemCommand()
        {
            if (_originalTitle != Title || _originalTime != Time)
            {
                _aimTimerService.AddAimTimer(_aimTimer.AimTimerModel);
                _originalTime = Time;
                _originalTitle = Title;
            }
        }

        public ICommand DeleteItemCommand
        {
            get
            {
                return new Command(async () => await ExecuteDeleteItemCommand());
            }
        }

        private async Task ExecuteDeleteItemCommand()
        {
            if (await _alertManager.DisplayAlert("Warning!", "Would you like to remove the timer completely?", "Yes", "No"))
            {
                _aimTimerService.DeleteAimTimer(_aimTimer.AimTimerModel.Id);
                _aimTimer.IsDeleted = true;
                await _navigation.PopAsync();
            }
        }

        #endregion

        public void Setup(IAimTimer aimTimer)
        {
            _aimTimer = aimTimer;

            _originalTime = Time;
            _originalTitle = Title;

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
