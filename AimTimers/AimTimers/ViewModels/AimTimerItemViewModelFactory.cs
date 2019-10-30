using AimTimers.Models;

namespace AimTimers.ViewModels
{
    public class AimTimerItemViewModelFactory : IAimTimerItemViewModelFactory
    {
        public IAimTimerItemViewModel Create(AimTimerItem aimTimerItem)
        {
            var result = new AimTimerItemViewModel();
            result.Setup(aimTimerItem);
            return result;
        }

        public IAimTimerItemViewModel CreateNew()
        {
            var aimTimerItem = new AimTimerItem
            {
                AimTimer = new AimTimer()
            };
            return Create(aimTimerItem);
        }
    }
}
