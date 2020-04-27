using System.Windows.Input;
using AimTimers.Bl;


namespace AimTimers.ViewModels
{
    public interface IAimTimerListItemViewModel
    {
        void Refresh();

        void RefreshTimeLeft();

        IAimTimer GetAimTimer();

        ICommand EditItemCommand { get; }

        ICommand DeleteItemCommand { get; }
    }
}
