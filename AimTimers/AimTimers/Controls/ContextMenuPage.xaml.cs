using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace AimTimers.Controls
{
    public partial class ContextMenuPage : PopupPage
    {
        private IEnumerable<ContextMenuItem> _items;
        public IEnumerable<ContextMenuItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public ContextMenuPage(int x, int y, IEnumerable<ContextMenuItem> items)
        {
            Items = items;

            InitializeComponent();

            //TranslationX = x;
            //TranslationY = y;
        }

        private async void ItemTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (sender is StackLayout senderControl && senderControl.BindingContext is ContextMenuItem contextMenuItem)
            {
                await Navigation.PopPopupAsync();
                contextMenuItem.Command.Execute(contextMenuItem.CommandParameter);
            }
        }

        private async void PageTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}