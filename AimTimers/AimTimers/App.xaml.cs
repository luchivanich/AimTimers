using AimTimers.ViewModels;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers
{
    public partial class App : Application
    {
        private readonly IViewFactory _viewFactory;
        private readonly IMainViewModel _mainViewModel;

        public App(IViewFactory viewFactory, IMainViewModel mainViewModel)
        {
            _viewFactory = viewFactory;
            _mainViewModel = mainViewModel;
        }

        public void Init()
        {
            InitializeComponent();
            MainPage = _viewFactory.CreatePage(_mainViewModel);
        }
    }
}
