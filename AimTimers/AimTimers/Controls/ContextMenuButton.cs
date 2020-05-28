using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace AimTimers.Controls
{
    public class ContextMenuButton : CustomButton
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

        public ContextMenuButton()
            : base()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += ShowContextMenu_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        private async void ShowContextMenu_Tapped(object sender, System.EventArgs e)
        {
            //var item = Items.First();
            //item.Command.Execute(item.CommandParameter);

            var coordinates = GetCoordinates?.Invoke();
            var contextMenuPage = new ContextMenuPage(coordinates?.x ?? default, coordinates?.y ?? default, Items);
            await Navigation.PushPopupAsync(contextMenuPage);
        }

        public Func<(int x, int y)> GetCoordinates = null;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            foreach(var item in Items)
            {
                item.BindingContext = this.BindingContext;
            }
        }
    }
}
