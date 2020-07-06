using AimTimers.Hotfixes;
using AimTimers.ViewModels;
using AimTimers.Views;
using Xamarin.Forms;

namespace AimTimers
{
    public partial class App : Application
    {
        private readonly IHotfixService _hotfixService;
        private readonly IViewFactory _viewFactory;
        private readonly IMainViewModel _mainViewModel;

        public App(IHotfixService hotfixService, IViewFactory viewFactory, IMainViewModel mainViewModel)
        {
            _hotfixService = hotfixService;
            _viewFactory = viewFactory;
            _mainViewModel = mainViewModel;
        }

        public void Init()
        {
            _hotfixService.ApplyHotfixes();

            _mainViewModel.Init();

            InitializeComponent();
            MainPage = _viewFactory.CreatePage(_mainViewModel);
        }
    }
}
