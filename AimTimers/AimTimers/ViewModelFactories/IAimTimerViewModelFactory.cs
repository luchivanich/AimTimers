using AimTimers.Models;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerViewModelFactory
    {
        IAimTimerViewModel Create(AimTimer aimTimer);
    }
}
