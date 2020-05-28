using System;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AimTimers.Views
{
    //[ContentProperty("PageContent")]
    public partial class BasePopupPage : PopupPage
    {
        public static readonly BindableProperty AcceptCommandProperty = BindableProperty.Create(
            nameof(AcceptCommand), 
            typeof(ICommand), 
            typeof(BasePopupPage),
            null);

        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        public View PageContent
        {
            get => PageContentContainer.Content;
            set
            {
                PageContentContainer.Content = value;
            }
        }

        public BasePopupPage()
        {
            InitializeComponent();
        }

        protected void Close_Tapped(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }
    }
}