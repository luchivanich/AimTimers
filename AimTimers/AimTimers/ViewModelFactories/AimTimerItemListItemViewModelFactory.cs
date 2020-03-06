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

        public AimTimerItemListItemViewModelFactory(IAimTimerService aimTimerService, IDateTimeProvider dateTimeProvider)
        {
            _aimTimerService = aimTimerService;
            _dateTimeProvider = dateTimeProvider;
        }

        public IAimTimerItemListItemViewModel Create(IAimTimer aimTimer, IAimTimerItem aimTimerItem)
        {
            var result = new AimTimerItemListItemViewModel(_aimTimerService, _dateTimeProvider);
            result.Setup(aimTimer, aimTimerItem);
            return result;
        }
    }
}
