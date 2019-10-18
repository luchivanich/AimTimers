using System;

using AimTimers.Models;

namespace AimTimers.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public AimTimer Item { get; set; }
        public ItemDetailViewModel(AimTimer item = null)
        {
            Title = item?.Title;
            Item = item;
        }
    }
}
