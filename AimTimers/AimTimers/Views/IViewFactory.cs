using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AimTimers.Views
{
    public interface IViewFactory
    {
        Page CreatePage(object viewModel);

        PopupPage CreatePopupPage(object viewModel);
    }
}
