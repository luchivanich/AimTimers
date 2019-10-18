using Xamarin.Forms;
using AimTimers.Services;
using AimTimers.Models;
using AimTimers.ViewModels;
using Unity;
using AimTimers.Views;
using System;

namespace AimTimers
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();

                var unityContainer = new UnityContainer();
                unityContainer.RegisterType<IDataStore<AimTimerItem>, MockDataStore>();
                unityContainer.RegisterType<IAimTimerService, AimTimerService>();
                unityContainer.RegisterType<IAimTimerItemViewModelFactory, AimTimerItemViewModelFactory>();
                unityContainer.RegisterType<IAimTimerTickService, AimTimerTickService>();
                unityContainer.RegisterType<IViewFactory, ViewFactory>();
                unityContainer.RegisterType<IAimTimersViewModel, AimTimersViewModel>();
                unityContainer.RegisterType<IMainPageViewModel, MainPageViewModel>();

                MainPage = new MainPage();
                MainPage.BindingContext = unityContainer.Resolve<IMainPageViewModel>();
            }
            catch (Exception e)
            {
                var i = 0;
            }
        }
    }
}
