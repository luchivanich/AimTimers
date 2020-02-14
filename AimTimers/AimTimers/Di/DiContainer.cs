using System;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Repository;
using AimTimers.Services;
using AimTimers.Utils;
using AimTimers.ViewModelFactories;
using AimTimers.ViewModels;
using AimTimers.Views;
using Unity;
using Xamarin.Forms;

namespace AimTimers.Di
{
    public class DiContainer
    {
        public UnityContainer SetupIoc()
        {
            var unityContainer = new UnityContainer();

            unityContainer.RegisterType<ITimer, TimerAdapter>();
            unityContainer.RegisterType<IDateTimeProvider, DateTimeProvider>();
            unityContainer.RegisterType<IViewFactory, ViewFactory>();
            unityContainer.RegisterType<INavigation, NavigationAdapter>();
            unityContainer.RegisterSingleton<IRepository, BaseRepository>();
            unityContainer.RegisterSingleton<IAimTimerService, AimTimerService>();
            unityContainer.RegisterSingleton<IAimTimerNotificationService, AimTimerNotificationService>();

            unityContainer.RegisterInstance<Func<AimTimerModel, IAimTimer>>(aimTimerModel => new AimTimer(aimTimerModel));

            unityContainer.RegisterFactory<Application>(c =>
            {
                var result = new App(
                    c.Resolve<IViewFactory>(),
                    c.Resolve<IMainViewModel>()
                );
                result.Init();
                return result;
            });
            
            unityContainer.RegisterType<IAimTimerItemViewModelFactory, AimTimerItemViewModelFactory>();
            unityContainer.RegisterType<IAimTimersViewModel, AimTimersViewModel>();
            unityContainer.RegisterType<IAimTimerViewModelFactory, AimTimerViewModelFactory>();

            unityContainer.RegisterFactory<IMainViewModel>(c =>
            {
                var result = new MainViewModel(
                    c.Resolve<IViewFactory>(),
                    c.Resolve<IAimTimersViewModel>()
                );
                result.Init();
                return result;
            });

            return unityContainer;
        }
    }
}
