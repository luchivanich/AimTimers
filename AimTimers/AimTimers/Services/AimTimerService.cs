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
            foreach (var aimTimerModel in aimTimerModels)
            {
                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                var indexForCurrentPeriod = aimTimer.GetIndexByDate(now);
                if (indexForCurrentPeriod < 0)
                {
                    continue;
                }

                var allItems = _repository
                    .LoadAllByKey<AimTimerItemModel>("aimTimerId", aimTimerModel.Id)
                    .Where(i => i.Index <= indexForCurrentPeriod)
                    .OrderByDescending(i => i.Index)
                    .ToList();

                var latestItem = allItems.FirstOrDefault();
                var startIndex = latestItem?.Index + 1 ?? GlobalConstants.START_INDEX;

                for (var aimTimerItemIndex = startIndex; aimTimerItemIndex < indexForCurrentPeriod; aimTimerItemIndex++)
                {
                    var period = aimTimer.GetPeriodByIndex(aimTimerItemIndex);
                    var itemToAdd = new AimTimerItemModel
                    {
                        AimTimerId = aimTimerModel.Id,
                        StartOfActivityPeriod = period.startDate,
                        EndOfActivityPeriod = period.endDate,
                        Ticks = aimTimerModel.Ticks,
                        PreviousInARow = latestItem.IsFinished ? latestItem.PreviousInARow + 1 : 0,
                        IsFinished = false,
                        Index = aimTimerItemIndex
                    };
                    _repository.Save(itemToAdd);
                    latestItem = itemToAdd;
                }

                result.Add(_aimTimerItemFactory.Invoke(aimTimer, latestItem));
            }
            return result;
        }

        public void SaveAimTimer(IAimTimerItem aimTimerItem)
        {
            var now = _dateTimeProvider.GetNow();

            var status = aimTimerItem.GetStatus();

            var aimTimerModel = aimTimerItem.AimTimer.GetAimTimerModel();
            var aimTimerItemModel = aimTimerItem.GetAimTimerItemModel();
            aimTimerItemModel.AimTimerId = aimTimerModel.Id;
            aimTimerItemModel.IsFinished = status.IsFinished;
            aimTimerItemModel.Index = aimTimerItem.AimTimer.GetIndexByDate(aimTimerItem.StartOfActivityPeriod);

            _repository.Save(aimTimerModel, aimTimerModel.Id);
            _repository.Save(aimTimerItemModel, aimTimerItemModel.Id);

            var itemsToUpdate = _repository
                .LoadAllByKey<AimTimerItemModel>("aimTimerId", aimTimerModel.Id)
                .Where(i => i.StartOfActivityPeriod > now)
                .OrderBy(i => i.StartOfActivityPeriod)
                .ToList();

            var inARow = aimTimerItemModel.IsFinished ? aimTimerItemModel.PreviousInARow + 1 : 0;

            foreach(var i in itemsToUpdate)
            {
                i.PreviousInARow = inARow;
                inARow = i.IsFinished ? i.PreviousInARow + 1 : 0;
                _repository.Save(i, i.Id);
            }

        }

        public void DeleteAimTimer(IAimTimer aimTimer)
        {
            var aimTimerModel = aimTimer.GetAimTimerModel();
            var aimTimerItemModels = _repository.LoadAllByKey<AimTimerItemModel>("aimTimerId", aimTimerModel.Id);
            foreach(var aimTimerItemModel in aimTimerItemModels)
            {
                _repository.Delete(aimTimerItemModel.Id);
            }
            _repository.Delete(aimTimerModel.Id);
        }

        public IEnumerable<IAimTimerItem> GetAimTimerItems(DateTime date)
        {
            return null;
        }
    }
}
