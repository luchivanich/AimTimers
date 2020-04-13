using System.Threading.Tasks;
using Xamarin.Forms;

namespace AimTimers.Utils
{
    public class AlertManager : IAlertManager
    {
        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
