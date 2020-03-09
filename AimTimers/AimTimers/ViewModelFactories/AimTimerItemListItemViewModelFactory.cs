using AimTimers.Bl;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public class AimTimerItemListItemViewModelFactory : IAimTimerItemListItemViewModelFactory
    {
        private readonly IAimTimerService _aimTimerService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAimTimerIntervalListItemViewModelFactory _aimTimerIntervalListItemViewModelFactory;

        public AimTimerItemListItemViewModelFactory(
            IAimTimerService aimTimerService,
            IDateTimeProvider dateTimeProvider,
            IAimTimerIntervalListItemViewModelFactory aimTimerIntervalListItemViewModelFactory)
        {
            _aimTimerService = aimTimerService;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerIntervalListItemViewModelFactory = aimTimerIntervalListItemViewModelFactory;
        }

        public IAimTimerItemListItemViewModel Create(IAimTimer aimTimer, IAimTimerItem aimTimerItem)
        {
            var result = new AimTimerItemListItemViewModel(_aimTimerService, _dateTimeProvider, _aimTimerIntervalListItemViewModelFactory);
            result.Setup(aimTimer, aimTimerItem);
            return result;
        }
    }
}
