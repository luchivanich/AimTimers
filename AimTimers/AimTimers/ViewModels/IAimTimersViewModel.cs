using System.Windows.Input;

namespace AimTimers.ViewModels
{
    public interface IAimTimersViewModel
    {
        ICommand RefreshCommand { get; }
        ICommand FreezeCommand { get; }
        ICommand AddItemCommand { get; }
        ICommand SelectItemCommand { get; }
    }
}
