namespace AimTimers.ViewModels
{
    public interface IAimTimerItemListItemViewModel
    {
        bool IsExpanded { get; set; }
        void RefreshTimeLeft();
    }
}
