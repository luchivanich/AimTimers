using AimTimers.Bl;


namespace AimTimers.ViewModels
{
    public interface IAimTimerListItemViewModel
    {
        void Refresh();
        IAimTimerItem GetAimTimerItem();
    }
}
