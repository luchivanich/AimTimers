using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimerInterval : IAimTimerInterval
    {
        private IAimTimerItem _aimTimerItem;

        public DateTime StartDate { get; set; }

        private DateTime? _endDate;
        public DateTime? EndDate 
        {
            get => _endDate == null ? (DateTime?)null : new DateTime(Math.Min(_endDate.Value.Ticks, _aimTimerItem.EndOfActivityPeriod.Ticks));
            set
            {
                _endDate = value;
            }
        }

        public AimTimerInterval(IAimTimerItem aimTimerItem)
        {
            _aimTimerItem = aimTimerItem;
        }

        public AimTimerIntervalModel GetAimTimerIntervalModel()
        {
            return new AimTimerIntervalModel
            {
                StartDate = StartDate,
                EndDate = EndDate
            };
        }
    }
}
