using AimTimers.Bl;
using AimTimers.ViewModels;

namespace AimTimers.ViewModelFactories
{
    public interface IAimTimerViewModelFactory
    {
        IAimTimerViewModel Create(IAimTimer aimTimer);
    }
}
