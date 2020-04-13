using System.Threading.Tasks;

namespace AimTimers.Utils
{
    public interface IAlertManager
    {
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    }
}
