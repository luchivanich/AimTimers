using AimTimers.Bl;


namespace AimTimers.ViewModels
{
    public interface IAimTimerItemViewModel
    {
        void RefreshTimeLeft();

        IAimTimer GetAimTimer();
    }
}
