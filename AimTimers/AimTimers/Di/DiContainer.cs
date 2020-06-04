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
            //unityContainer.RegisterType<IDateTimeProvider, DateTimeProvider>();
            unityContainer.RegisterInstance<IDateTimeProvider>(new DateTimeProvider());
            unityContainer.RegisterInstance(DependencyService.Get<INotificationManager>());
            unityContainer.RegisterType<IViewFactory, ViewFactory>();
            unityContainer.RegisterType<INavigation, NavigationAdapter>();
            unityContainer.RegisterType<IAlertManager, AlertManager>();
            unityContainer.RegisterInstance(MessagingCenter.Instance);
            unityContainer.RegisterSingleton<IRepository, BaseRepository>();
            unityContainer.RegisterSingleton<IAimTimerService, AimTimerService>();
            unityContainer.RegisterType<IAimTimerNotificationService, AimTimerNotificationService>();

            unityContainer.RegisterFactory<Func<AimTimerModel, IAimTimer>>(
                container => new Func<AimTimerModel, IAimTimer>(
                    aimTimerModel => new AimTimer(aimTimerModel, container.Resolve<IDateTimeProvider>())
                )
            );
            unityContainer.RegisterFactory<Func<IAimTimer, AimTimerItemModel, IAimTimerItem>>(
                container => new Func<IAimTimer, AimTimerItemModel, IAimTimerItem>(
                    (aimTimer, aimTimerItemModel) => new AimTimerItem(aimTimer, aimTimerItemModel, container.Resolve<IDateTimeProvider>())
                )
            );
            unityContainer.RegisterFactory<Func<IAimTimerItem, AimTimerIntervalModel, IAimTimerInterval>>(
                container => new Func<IAimTimerItem, AimTimerIntervalModel, IAimTimerInterval>(
                    (aimTimerItem, aimTimerIntervalModel) => new AimTimerInterval(aimTimerItem, aimTimerIntervalModel)
                )
            );

            unityContainer.RegisterFactory<Application>(container =>
            {
                var result = new App(
                    container.Resolve<IViewFactory>(),
                    container.Resolve<IMainViewModel>()
                );
                result.Init();
                return result;
            });
            
            unityContainer.RegisterType<IAimTimerListItemViewModelFactory, AimTimerListItemViewModelFactory>();
            unityContainer.RegisterFactory<IAimTimersViewModel>(c =>
            {
                var result = new AimTimersViewModel(
                    c.Resolve<IDateTimeProvider>(),
                    c.Resolve<IAimTimerNotificationService>(),
                    c.Resolve<INavigation>(),
                    c.Resolve<IAlertManager>(),
                    c.Resolve<IMessagingCenter>(),
                    c.Resolve<IViewFactory>(),
                    c.Resolve<IAimTimerService>(),
                    c.Resolve<IAimTimerListItemViewModelFactory>(),
                    c.Resolve<IAimTimerViewModelFactory>(),
                    c.Resolve<Func<AimTimerModel, IAimTimer>>());
                result.Init();
                return result;
            });
            unityContainer.RegisterType<IAimTimerViewModelFactory, AimTimerViewModelFactory>();
            unityContainer.RegisterType<IAimTimerIntervalListItemViewModel, AimTimerIntervalListItemViewModel>();
            unityContainer.RegisterType<IAimTimerIntervalListItemViewModelFactory, AimTimerIntervalListItemViewModelFactory>();

            unityContainer.RegisterType<IAimTimerIntervalViewModel, AimTimerIntervalViewModel>();
            unityContainer.RegisterFactory<Func<IAimTimerInterval, IAimTimerIntervalViewModel>>(
                container => new Func<IAimTimerInterval, IAimTimerIntervalViewModel>(
                    aimTimerInterval =>
                    {
                        var result = new AimTimerIntervalViewModel(
                            container.Resolve<IDateTimeProvider>(),
                            container.Resolve<INavigation>(),
                            container.Resolve<IMessagingCenter>(),
                            container.Resolve<IAimTimerService>());
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
