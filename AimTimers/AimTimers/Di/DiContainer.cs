using System;
using AimTimers.Bl;
using AimTimers.Hotfixes;
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
            unityContainer.RegisterInstance<IDateTimeProvider>(new DateTimeProvider());
            unityContainer.RegisterInstance(DependencyService.Get<INotificationManager>());
            unityContainer.RegisterType<IViewFactory, ViewFactory>();
            unityContainer.RegisterType<INavigation, NavigationAdapter>();
            unityContainer.RegisterType<IAlertManager, AlertManager>();
            unityContainer.RegisterInstance(MessagingCenter.Instance);
            unityContainer.RegisterSingleton<IRepository, AimTimerRepository>();
            unityContainer.RegisterSingleton<IRepository, HotfixRepository>("Hotfix");
            unityContainer.RegisterSingleton<IAimTimerService, AimTimerService>();
            unityContainer.RegisterType<IAimTimerNotificationService, AimTimerNotificationService>();

            unityContainer.RegisterFactory<IHotfixService>(container =>
              {
                  var result = new HotfixService(
                      container.Resolve<IRepository>("Hotfix"),
                      container.Resolve<IRepository>()
                  );
                  result.Init();
                  return result;
              });

            unityContainer.RegisterFactory<Func<AimTimerModel, IAimTimer>>(
                container => new Func<AimTimerModel, IAimTimer>(
                    aimTimerModel =>
                    {
                        var result = new AimTimer(aimTimerModel);
                        result.Init();
                        return result;
                    }
                )
            );
            unityContainer.RegisterFactory<Func<IAimTimer, AimTimerItemModel, IAimTimerItem>>(
                container => new Func<IAimTimer, AimTimerItemModel, IAimTimerItem>(
                    (aimTimer, aimTimerItemModel) =>
                    {
                        var result = new AimTimerItem(
                            aimTimer, 
                            aimTimerItemModel, 
                            container.Resolve<IDateTimeProvider>(),
                            container.Resolve<Func<DateTime, DateTime?, IAimTimerInterval>>());
                        result.Init();
                        return result;
                    }
                )
            );
            unityContainer.RegisterFactory<Func<DateTime, DateTime?, IAimTimerInterval>>(
                container => new Func<DateTime, DateTime?, IAimTimerInterval>(
                    (startDate, endDate) => new AimTimerInterval { StartDate = startDate, EndDate = endDate }
                )
            );

            unityContainer.RegisterFactory<Application>(container =>
            {
                var result = new App(
                    container.Resolve<IHotfixService>(),
                    container.Resolve<IViewFactory>(),
                    container.Resolve<IMainViewModel>()
                ); ;
                result.Init();
                return result;
            });
            
            unityContainer.RegisterType<IAimTimerListItemViewModelFactory, AimTimerListItemViewModelFactory>();
            unityContainer.RegisterFactory<IAimTimersViewModel>(container =>
            {
                var result = new AimTimersViewModel(
                    container.Resolve<IDateTimeProvider>(),
                    container.Resolve<IAimTimerNotificationService>(),
                    container.Resolve<INavigation>(),
                    container.Resolve<IAlertManager>(),
                    container.Resolve<IMessagingCenter>(),
                    container.Resolve<IViewFactory>(),
                    container.Resolve<IAimTimerService>(),
                    container.Resolve<IAimTimerListItemViewModelFactory>(),
                    container.Resolve<IAimTimerViewModelFactory>(),
                    container.Resolve<Func<AimTimerModel, IAimTimer>>(),
                    container.Resolve<Func<IAimTimer, AimTimerItemModel, IAimTimerItem>>());
                //result.Init();
                return result;
            });
            unityContainer.RegisterType<IAimTimerViewModelFactory, AimTimerViewModelFactory>();
            unityContainer.RegisterType<IAimTimerIntervalListItemViewModel, AimTimerIntervalListItemViewModel>();
            unityContainer.RegisterType<IAimTimerIntervalListItemViewModelFactory, AimTimerIntervalListItemViewModelFactory>();

            unityContainer.RegisterType<IAimTimerIntervalViewModel, AimTimerIntervalViewModel>();
            
            unityContainer.RegisterFactory<Func<IAimTimerItem, IAimTimerInterval, IAimTimerIntervalViewModel>>(
                container => new Func<IAimTimerItem, IAimTimerInterval, IAimTimerIntervalViewModel>(
                    (aimTimerItem, aimTimerInterval) =>
                    {
                        var result = new AimTimerIntervalViewModel(
                            container.Resolve<IDateTimeProvider>(),
                            container.Resolve<INavigation>(),
                            container.Resolve<IMessagingCenter>(),
                            container.Resolve<IAimTimerService>());
                        result.Setup(aimTimerItem, aimTimerInterval);
                        return result;
                    }
                )
            );

            unityContainer.RegisterFactory<IMainViewModel>(containter =>
            {
                var result = new MainViewModel(
                    containter.Resolve<IViewFactory>(),
                    containter.Resolve<IAimTimersViewModel>()
                );
                //result.Init();
                return result;
            });

            return unityContainer;
        }
    }
}
