using Xamarin.Forms;
using AimTimers.Services;
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

                MainPage = new MainPage();

                var unityContainer = new UnityContainer();
                unityContainer.RegisterInstance(Current.MainPage.Navigation);
                unityContainer.RegisterSingleton<IAimTimerService, AimTimerService>();

                unityContainer.RegisterType<IViewFactory, ViewFactory>();
                unityContainer.RegisterType<IAimTimerItemViewModelFactory, AimTimerItemViewModelFactory>();
                unityContainer.RegisterType<IAimTimerViewModelFactory, AimTimerViewModelFactory>();

                unityContainer.RegisterType<IAimTimersViewModel, AimTimersViewModel>();
                unityContainer.RegisterType<IMainPageViewModel, MainPageViewModel>();

                MainPage.BindingContext = unityContainer.Resolve<IMainPageViewModel>();
            }
            catch (Exception e)
            {
                var i = 0;
            }
        }
    }
}
