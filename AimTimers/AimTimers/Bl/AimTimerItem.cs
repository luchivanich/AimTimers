using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Models;
using AimTimers.Utils;

namespace AimTimers.Bl
{
    public class AimTimerItem : IAimTimerItem
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Func<IAimTimerItem, DateTime, DateTime?, IAimTimerInterval> _aimTimerIntervalFactory;

        private AimTimerItemModel _aimTimerItemModel;

        public IAimTimer AimTimer { get; }
        public long Ticks { get; set; }
        public List<IAimTimerInterval> AimTimerIntervals { get; set; } = new List<IAimTimerInterval>();
        public DateTime StartOfActivityPeriod { get; set; }
        public DateTime EndOfActivityPeriod { get; set; }
        public bool IsFinished { get; set; }
        public int InARow { get; set; }

        public AimTimerItem(
            IAimTimer aimTimer,
            AimTimerItemModel aimTimerItemModel,
            IDateTimeProvider dateTimeProvider,
            Func<IAimTimerItem, DateTime, DateTime?, IAimTimerInterval> aimTimerIntervalFactory)
        {
            AimTimer = aimTimer;
            _aimTimerItemModel = aimTimerItemModel;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerIntervalFactory = aimTimerIntervalFactory;
        }

        public void Init()
        {
            Ticks = _aimTimerItemModel.Ticks;
            foreach (var aimTimerIntervalModel in _aimTimerItemModel.AimTimerIntervals)
            {
                var aimTimerInterval = _aimTimerIntervalFactory.Invoke(this, aimTimerIntervalModel.StartDate, aimTimerIntervalModel.EndDate);
                AimTimerIntervals.Add(aimTimerInterval);
            }
            StartOfActivityPeriod = _aimTimerItemModel.StartOfActivityPeriod;
            EndOfActivityPeriod = _aimTimerItemModel.EndOfActivityPeriod;
            IsFinished = _aimTimerItemModel.IsFinished;
            InARow = _aimTimerItemModel.InARow;
        }

        public AimTimerItemStatus GetStatus()
        {
            var now = _dateTimeProvider.GetNow();
            var result = new AimTimerItemStatus()
            {
                StatusFlags = AimTimerStatusFlags.None,
                TimeLeft = new TimeSpan(Ticks) - new TimeSpan(AimTimerIntervals.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks))
            };
            
            var interval = AimTimerIntervals
                .Where(i => i.StartDate <= now && i.EndDate >= now || i.EndDate == null)
                .OrderByDescending(i => i.EndDate ?? DateTime.MaxValue)
                .FirstOrDefault();

            if (interval != null && interval.EndDate == null)
            {
                result.StatusFlags |= AimTimerStatusFlags.Running;
            }

            if (result.TimeLeft.Ticks > 0)
            {
                result.StatusFlags |= AimTimerStatusFlags.Active;
            }

            result.IsFinished = result.TimeLeft.Ticks <= 0;
            result.InARow = InARow + (result.IsFinished ? 1 : 0);

            return result;
        }

        public void Start()
        {
            var now = _dateTimeProvider.GetNow();
            if (now < StartOfActivityPeriod || now > EndOfActivityPeriod || AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            var aimTimerInterval = _aimTimerIntervalFactory.Invoke(this, now, null);
            AimTimerIntervals.Add(aimTimerInterval);
        }

        public void Stop()
        {
            var now = _dateTimeProvider.GetNow();
            var lastInterval = AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = now;
        }

        public AimTimerItemModel GetAimTimerItemModel()
        {
            var aimTimerModel = AimTimer.GetAimTimerModel();

            var aimTimerIntervalModels = new List<AimTimerIntervalModel>();
            foreach(var aimTimerInterval in AimTimerIntervals)
            {
                aimTimerIntervalModels.Add(new AimTimerIntervalModel
                {
                    StartDate = aimTimerInterval.StartDate,
                    EndDate = aimTimerInterval.EndDate
                });
            }

            return new AimTimerItemModel
            {
                Id = _aimTimerItemModel.Id,
                AimTimerId = aimTimerModel.Id,
                Ticks = Ticks,
                AimTimerIntervals = aimTimerIntervalModels,
                StartOfActivityPeriod = StartOfActivityPeriod,
                EndOfActivityPeriod = EndOfActivityPeriod
            };
        }
    }
}
