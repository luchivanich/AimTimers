
using AimTimers.Models;

namespace AimTimers.ViewModels
{
    public interface IAimTimerViewModelFactory
    {
        IAimTimerViewModel Create(AimTimer aimTimer);

        IAimTimerViewModel CreateNew();
    }
}
