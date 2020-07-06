using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Repository;
using AimTimers.Utils;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private readonly IRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;
        private readonly Func<IAimTimer, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;

        public AimTimerService(
            IRepository repository,
            IDateTimeProvider dateTimeProvider,
            Func<AimTimerModel,IAimTimer> aimTimerFactory,
            Func<IAimTimer, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerFactory = aimTimerFactory;
            _aimTimerItemFactory = aimTimerItemFactory;
        }

        public IEnumerable<IAimTimerItem> GetActiveAimTimers()
        {
            var now = _dateTimeProvider.GetNow();
            var aimTimerModels = _repository.LoadAll<AimTimerModel>();
            var result = new List<IAimTimerItem>();
            foreach(var aimTimerModel in aimTimerModels)
            {
                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                var tmp = _repository
                    .LoadAll<AimTimerItemModel>();
                var itemToAdd = _repository
                    .LoadAll<AimTimerItemModel>()
                    .FirstOrDefault(i => i.StartOfActivityPeriod <= now && i.EndOfActivityPeriod >= now);

                if (itemToAdd == null)
                {
                    itemToAdd = new AimTimerItemModel
                    {
                        StartOfActivityPeriod = _dateTimeProvider.GetStartOfTheDay(),
                        EndOfActivityPeriod = _dateTimeProvider.GetEndOfTheDay(),
                        Ticks = aimTimerModel.Ticks
                    };
                }

                result.Add(_aimTimerItemFactory.Invoke(aimTimer, itemToAdd));
            }
            return result;
        }

        public void AddAimTimer(IAimTimerItem aimTimerItem)
        {
            _repository.Save(aimTimerItem.AimTimerModel, aimTimerItem.AimTimerModel.Id);
            aimTimerItem.AimTimerItemModel.AimTimerId = aimTimerItem.AimTimerModel.Id;
            _repository.Save(aimTimerItem.AimTimerItemModel, aimTimerItem.AimTimerItemModel.Id);
        }

        public void DeleteAimTimer(string aimTimerId)
        {
            _repository.Delete(aimTimerId);
        }

        public IEnumerable<IAimTimerItem> GetAimTimerItems(DateTime date)
        {
            return null;
        }
    }
}
