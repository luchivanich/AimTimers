using AimTimers.ViewModelFactories;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        private readonly IViewFactory _viewFactory;
        private readonly IAimTimersViewModel _aimTimersViewModel;

        private DataTemplate _itemsTab;
        public DataTemplate ItemsTab
        {
            get => _itemsTab;
            set
            {
                _itemsTab = value;
                OnPropertyChanged();
            }
        }

        private DataTemplate _archiveItemsTab;
        public DataTemplate ArchiveItemsTab
        {
            get => _archiveItemsTab;
            set
            {
                _archiveItemsTab = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IViewFactory viewFactory, IAimTimersViewModel aimTimersViewModel)
        {
            _viewFactory = viewFactory;
            _aimTimersViewModel = aimTimersViewModel;
        }

        public void Init()
        {
            ItemsTab = new DataTemplate(() => { return _viewFactory.CreatePage(_aimTimersViewModel); });
            ArchiveItemsTab = new DataTemplate(() => { return new AboutPage(); });
        }
    }
}
