using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AimTimers.Utils
{
    public class NavigationAdapter : INavigation
    {
        private INavigation _mainPageNavigation;
        private INavigation MainPageNavigation
        {
            get
            {
                if (_mainPageNavigation == null && Application.Current?.MainPage?.Navigation != null)
                {
                    _mainPageNavigation = Application.Current?.MainPage?.Navigation;
                }
                return _mainPageNavigation;
            }
        }

        public IReadOnlyList<Page> ModalStack => throw new System.NotImplementedException();

        public IReadOnlyList<Page> NavigationStack => MainPageNavigation.NavigationStack;

        public void InsertPageBefore(Page page, Page before)
        {
            throw new System.NotImplementedException();
        }

        public Task<Page> PopAsync()
        {
            return MainPageNavigation.PopAsync();
        }

        public Task<Page> PopAsync(bool animated)
        {
            return MainPageNavigation.PopAsync(animated);
        }

        public Task<Page> PopModalAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Page> PopModalAsync(bool animated)
        {
            throw new System.NotImplementedException();
        }

        public Task PopToRootAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task PopToRootAsync(bool animated)
        {
            throw new System.NotImplementedException();
        }

        public Task PushAsync(Page page)
        {
            return MainPageNavigation.PushAsync(page);
        }

        public Task PushAsync(Page page, bool animated)
        {
            return MainPageNavigation.PushAsync(page, animated);
        }

        public Task PushModalAsync(Page page)
        {
            throw new System.NotImplementedException();
        }

        public Task PushModalAsync(Page page, bool animated)
        {
            throw new System.NotImplementedException();
        }

        public void RemovePage(Page page)
        {
            throw new System.NotImplementedException();
        }
    }
}
