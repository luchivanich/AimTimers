using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AimTimers.Views
{
    public interface IViewFactory
    {
        Page CreatePage(object viewModel);

        Task NavigatePageAsync(object viewModel);
    }
}
