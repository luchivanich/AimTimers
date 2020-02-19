using System;
using System.Collections.Generic;
using AimTimers.Utils;
using System.Linq;
using AimTimers.Bl;

namespace AimTimers.Services
{
    public class AimTimerNotificationService : IAimTimerNotificationService
    {
        private static object _lock = new object();

        private readonly ITimer _timer;

        public event EventHandler<AimTimersEventArgs> OnStatusChanged;

        private IEnumerable<IAimTimer> _aimTimers = new List<IAimTimer>();

        public AimTimerNotificationService(ITimer timer)
        {
            _timer = timer;
        }

        public void Start()
        {
            _timer.Interval = 100;
            _timer.Elapsed -= OnTimedEvent;
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void SetItemsToFollow(IEnumerable<IAimTimer> aimTimers)
        {
            lock (_lock)
            {
                _aimTimers = aimTimers;
            }
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                if (_aimTimers == null || _aimTimers.Count() == 0)
                {
                    return;
                }

                OnStatusChanged?.Invoke(this, new AimTimersEventArgs { AimTimers = _aimTimers });
            }
        }
    }
}
