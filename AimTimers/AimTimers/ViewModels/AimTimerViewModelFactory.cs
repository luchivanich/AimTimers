using AimTimers.Models;

namespace AimTimers.ViewModels
{
    public class AimTimerViewModelFactory : IAimTimerViewModelFactory
    {
        public IAimTimerViewModel Create(AimTimer aimTimer)
        {
            var result = new AimTimerViewModel();
            result.Setup(aimTimer);
            return result;
        }

        public IAimTimerViewModel CreateNew()
        {
            var aimTimer = new AimTimer();
            return Create(aimTimer);
        }
    }
}
