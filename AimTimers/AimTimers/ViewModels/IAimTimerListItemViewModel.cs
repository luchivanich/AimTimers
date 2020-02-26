using AimTimers.Bl;


namespace AimTimers.ViewModels
{
    public interface IAimTimerListItemViewModel
    {
        void RefreshTimeLeft();

        IAimTimer GetAimTimer();
    }
}
