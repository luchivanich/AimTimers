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
        private readonly Func<DateTime, DateTime?, IAimTimerInterval> _aimTimerIntervalFactory;

        private AimTimerItemModel _aimTimerItemModel;

        public IAimTimer AimTimer { get; }
        public long Ticks { get; set; }
        public List<IAimTimerInterval> AimTimerIntervals { get; set; } = new List<IAimTimerInterval>();
        public DateTime StartOfActivityPeriod { get; set; }
        public DateTime EndOfActivityPeriod { get; set; }

        public AimTimerItem(
            IAimTimer aimTimer,
            AimTimerItemModel aimTimerItemModel,
            IDateTimeProvider dateTimeProvider,
            Func<DateTime, DateTime?, IAimTimerInterval> aimTimerIntervalFactory)
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
                var aimTimerInterval = _aimTimerIntervalFactory.Invoke(aimTimerIntervalModel.StartDate, aimTimerIntervalModel.EndDate);
                AimTimerIntervals.Add(aimTimerInterval);
            }
            StartOfActivityPeriod = _aimTimerItemModel.StartOfActivityPeriod;
            EndOfActivityPeriod = _aimTimerItemModel.EndOfActivityPeriod;
        }

        public void Refresh()
        {
            foreach (var item in AimTimerIntervals.Where(i => i.EndDate > EndOfActivityPeriod || (i.EndDate == null && DateTime.Now > EndOfActivityPeriod)))
            {
                item.EndDate = EndOfActivityPeriod;
            }
        }

        public TimeSpan GetTimeLeft()
        {
            var now = _dateTimeProvider.GetNow();
            Refresh();
            return new TimeSpan(Ticks) - new TimeSpan(AimTimerIntervals.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks));
        }

        public void Start()
        {
            var now = _dateTimeProvider.GetNow();
            if (now < StartOfActivityPeriod || now > EndOfActivityPeriod || AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            var aimTimerInterval = _aimTimerIntervalFactory.Invoke(now, null);
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

        public AimTimerStatusFlags GetAimTimerStatusFlags()
        {
            var result = AimTimerStatusFlags.None;
            var now = _dateTimeProvider.GetNow();
            Refresh();
            var interval = AimTimerIntervals.FirstOrDefault(i => i.StartDate <= now && i.EndDate >= now || i.EndDate == null);
            if (interval != null && interval.EndDate == null)
            {
                result |= AimTimerStatusFlags.Running;
            }
            if (GetTimeLeft().Ticks > 0)
            {
                result |= AimTimerStatusFlags.Active;
            }
            return result;
        }

        public AimTimerItemModel GetAimTimerItemModel()
        {
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
                AimTimerId = _aimTimerItemModel.AimTimerId,
                Ticks = Ticks,
                AimTimerIntervals = aimTimerIntervalModels,
                StartOfActivityPeriod = StartOfActivityPeriod,
                EndOfActivityPeriod = EndOfActivityPeriod
            };
        }
    }
}
