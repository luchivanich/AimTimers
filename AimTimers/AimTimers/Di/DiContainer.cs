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
            unityContainer.RegisterInstance(DependencyService.Get<INotificationManager>());
            unityContainer.RegisterType<IViewFactory, ViewFactory>();
            unityContainer.RegisterType<INavigation, NavigationAdapter>();
            unityContainer.RegisterType<IAlertManager, AlertManager>();
            unityContainer.RegisterSingleton<IRepository, BaseRepository>();
            unityContainer.RegisterSingleton<IAimTimerService, AimTimerService>();
            unityContainer.RegisterType<IAimTimerNotificationService, AimTimerNotificationService>();

            unityContainer.RegisterFactory<Func<AimTimerModel, IAimTimer>>(
                container => new Func<AimTimerModel, IAimTimer>(
                    aimTimerModel => new AimTimer(aimTimerModel, container.Resolve<IDateTimeProvider>())
                )
            );
            unityContainer.RegisterFactory<Func<AimTimerModel, AimTimerItemModel, IAimTimerItem>>(
                container => new Func<AimTimerModel, AimTimerItemModel, IAimTimerItem>(
                    (aimTimer, aimTimerItemModel) => new AimTimerItem(aimTimer, aimTimerItemModel, container.Resolve<IDateTimeProvider>())
                )
            );
            unityContainer.RegisterFactory<Func<AimTimerIntervalModel, IAimTimerInterval>>(
                container => new Func<AimTimerIntervalModel, IAimTimerInterval>(
                    (aimTimerIntervalModel) => new AimTimerInterval(aimTimerIntervalModel)
                )
            );

            unityContainer.RegisterFactory<Application>(c =>
            {
                var result = new App(
                    c.Resolve<IViewFactory>(),
                    c.Resolve<IMainViewModel>()
                );
                result.Init();
                return result;
            });
            
            unityContainer.RegisterType<IAimTimerListItemViewModelFactory, AimTimerListItemViewModelFactory>();
            unityContainer.RegisterType<IAimTimersViewModel, AimTimersViewModel>();
            unityContainer.RegisterType<IAimTimerViewModelFactory, AimTimerViewModelFactory>();
            unityContainer.RegisterType<IAimTimerItemListItemViewModel, AimTimerItemListItemViewModel>();
            unityContainer.RegisterType<IAimTimerItemListItemViewModelFactory, AimTimerItemListItemViewModelFactory>();
            unityContainer.RegisterType<IAimTimerIntervalListItemViewModel, AimTimerIntervalListItemViewModel>();
            unityContainer.RegisterType<IAimTimerIntervalListItemViewModelFactory, AimTimerIntervalListItemViewModelFactory>();

            unityContainer.RegisterType<IAimTimerIntervalViewModel, AimTimerIntervalViewModel>();
            unityContainer.RegisterFactory<Func<IAimTimerInterval, IAimTimerIntervalViewModel>>(
                container => new Func<IAimTimerInterval, IAimTimerIntervalViewModel>(
                    aimTimerInterval =>
                    {
                        var result = new AimTimerIntervalViewModel();
                        result.Setup(aimTimerInterval);
                        return result;
                    }
                )
            );


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
