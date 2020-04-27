using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AimTimers.Views
{
    public class ViewFactory : IViewFactory
    {
        private readonly INavigation _navigation;

        public ViewFactory(INavigation navigation)
        {
            _navigation = navigation;
        }

        public Page CreatePage(object viewModel)
        {
            return CreateGenericPage<Page>(viewModel);
        }

        public PopupPage CreatePopupPage(object viewModel)
        {
            return CreateGenericPage<PopupPage>(viewModel);
        }

        private T CreateGenericPage<T>(object viewModel) where T: Page
        {
            var viewModelClassName = viewModel.GetType().Name;
            var pageClassName = viewModelClassName.Replace("ViewModel", "Page");
            var asm = viewModel.GetType().Assembly;
            var pageType = asm.GetType($"{this.GetType().Namespace}.{pageClassName}");
            var result = Activator.CreateInstance(pageType) as T;
            result.BindingContext = viewModel;
            return result;
        }
    }
}
