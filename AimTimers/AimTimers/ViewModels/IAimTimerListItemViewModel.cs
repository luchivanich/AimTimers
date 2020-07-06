using AimTimers.Bl;


namespace AimTimers.ViewModels
{
    public interface IAimTimerListItemViewModel
    {
        void Refresh();
        void RefreshTimeLeft();
        IAimTimerItem GetAimTimerItem();
    }
}
