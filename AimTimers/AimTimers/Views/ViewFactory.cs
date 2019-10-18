using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AimTimers.Views
{
    public class ViewFactory : IViewFactory
    {
        public Page CreatePage(object viewModel)
        {
            var viewModelClassName = viewModel.GetType().Name;
            var pageClassName = viewModelClassName.Replace("ViewModel", "Page");
            var asm = viewModel.GetType().Assembly;
            var pageType = asm.GetType($"{this.GetType().Namespace}.{pageClassName}");
            var result = Activator.CreateInstance(pageType) as Page;
            result.BindingContext = viewModel;
            return result;
        }

        public async Task NavigatePageAsync(object viewModel)
        {
            var page = CreatePage(viewModel);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
