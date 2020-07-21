using System;
using System.Linq;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Repository;
using AimTimers.Utils;

namespace AimTimers.Hotfixes
{
    public class Hotfix_02_SetOriginDateAndIndexes : IHotfix
    {
        private DateTime ORIGIN_DATE = new DateTime(2020, 7, 1, 12, 0, 0);

        public string HotfixId => "Hotfix_02_SetOriginDateAndIndexes";

        private readonly IRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;
        private readonly Func<IAimTimer, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;

        public Hotfix_02_SetOriginDateAndIndexes(
            IRepository repository,
            IDateTimeProvider dateTimeProvider,
            Func<AimTimerModel, IAimTimer> aimTimerFactory,
            Func<IAimTimer, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerFactory = aimTimerFactory;
            _aimTimerItemFactory = aimTimerItemFactory;
        }

        public void Apply()
        {
            var now = _dateTimeProvider.GetNow();

            var aimTimerModels = _repository.LoadAll<AimTimerModel>();
            foreach(var aimTimerModel in aimTimerModels)
            {
                if (aimTimerModel.OriginDate > ORIGIN_DATE)
                {
                    continue;
                }

                var aimTimer = _aimTimerFactory.Invoke(aimTimerModel);
                var todaysIndex = aimTimer.GetIndexByDate(now);

                var aimTimerItemModels = _repository
                    .LoadAllByKey<AimTimerItemModel>("aimTimerId", aimTimerModel.Id)
                    .OrderBy(i => i.StartOfActivityPeriod)
                    .Select(i => new { index = aimTimer.GetIndexByDate(i.StartOfActivityPeriod), aimTimerItemModel = i})
                    .ToList();

                var originDate = aimTimerItemModels.FirstOrDefault()?.aimTimerItemModel.StartOfActivityPeriod.Date;
                aimTimerModel.OriginDate = originDate ?? ORIGIN_DATE;
                _repository.Save(aimTimerModel);

                var previousInARow = 0;

                for (var index = GlobalConstants.START_INDEX; index <= todaysIndex; index++)
                {
                    var aimTimerItemModel = aimTimerItemModels.FirstOrDefault(i => i.index == index)?.aimTimerItemModel;
                    if (aimTimerItemModel == null)
                    {
                        var period = aimTimer.GetPeriodByIndex(index);
                        aimTimerItemModel = new AimTimerItemModel
                        {
                            AimTimerId = aimTimerModel.Id,
                            Ticks = aimTimerModel.Ticks,
                            StartOfActivityPeriod = period.startDate,
                            EndOfActivityPeriod = period.endDate
                        };
                    }

                    var aimTimerItem = _aimTimerItemFactory.Invoke(aimTimer, aimTimerItemModel);
                    var status = aimTimerItem.GetStatus();

                    aimTimerItemModel.Index = index;
                    aimTimerItemModel.IsFinished = status.IsFinished;
                    aimTimerItemModel.PreviousInARow = previousInARow;
                    _repository.Save(aimTimerItemModel);

                    previousInARow = status.IsFinished ? previousInARow + 1 : 0;
                }
            }
        }
    }
}
