using System;
using System.Collections.Generic;
using static System.Math;
using AimTimers.Utils;
using System.Linq;
using AimTimers.Bl;

namespace AimTimers.Services
{
    public class AimTimerNotificationService : IAimTimerNotificationService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAimTimerService _aimTimerService;
        private readonly ITimer _timer;

        public event EventHandler<AimTimersEventArgs> OnStatusChanged;

        private IEnumerable<IAimTimer> _aimTimers;

        public AimTimerNotificationService(IDateTimeProvider dateTimeProvider, IAimTimerService aimTimerService, ITimer timer)
        {
            _dateTimeProvider = dateTimeProvider;
            _aimTimerService = aimTimerService;
            _timer = timer;
        }

        public void Start()
        {
            SetupTimer();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void SetupTimer()
        {
            var interval = CalculateTimeToNextEvent();
            if (interval == 0)
            {
                return;
            }

            _timer.Interval = interval;
            _timer.Elapsed -= OnTimedEvent;
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        private double CalculateTimeToNextEvent()
        {
            _aimTimers = _aimTimerService.GetActiveAimTimers();
            if (_aimTimers.Count() == 0)
            {
                return 0;
            }
            return Max(_aimTimers.Min(i => CalculateTimeToNextEvent(i)), 100);
        }

        private double CalculateTimeToNextEvent(IAimTimer aimTimer)
        {
            var now = _dateTimeProvider.GetNow();
            var currentAimTimerItem = aimTimer.GetAimTimerItemByDate(now);
            var timeLeft = GetTimeLeft(aimTimer, currentAimTimerItem, now);
            var timeToExpiration = GetTimeToExpiration(currentAimTimerItem, now);
            return Min(timeLeft, timeToExpiration);
        }

        private double GetTimeLeft(IAimTimer aimTimer, IAimTimerItem currentAimTimerItem, DateTime now)
        {
            return (new TimeSpan(aimTimer.AimTimerModel.Ticks ?? 0) - new TimeSpan(currentAimTimerItem.AimTimerItemModel.AimTimerIntervals?.Sum(i => (i.EndDate ?? now).Ticks - i.StartDate.Ticks) ?? 0)).TotalMilliseconds;
        }

        private double GetTimeToExpiration(IAimTimerItem currentAimTimerItem, DateTime now)
        {
            return (currentAimTimerItem.AimTimerItemModel.EndOfActivityPeriod - now).TotalMilliseconds;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            var itemsForEvent = _aimTimers.Where(i => CalculateTimeToNextEvent(i) <= 0).ToList();
            OnStatusChanged?.Invoke(this, new AimTimersEventArgs { AimTimers = itemsForEvent });

            SetupTimer();
        }
    }
}
