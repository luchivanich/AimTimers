using System;
using Xamarin.Forms;
using AimTimers.Models;

namespace AimTimers.Views
{
    public partial class NewItemPage : ContentPage
    {
        public AimTimer Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new AimTimer
            {
                Title = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}