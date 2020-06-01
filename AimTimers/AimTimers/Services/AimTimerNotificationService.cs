using System;
using System.Collections.Generic;
using AimTimers.Utils;
using System.Linq;
using AimTimers.Bl;
using AsyncAwaitBestPractices;

namespace AimTimers.Services
{
    public class AimTimerNotificationService : IAimTimerNotificationService
    {
        private static object _lock = new object();

        private readonly ITimer _timer;

        readonly WeakEventManager<AimTimersEventArgs> _statusChangedEventManager = new WeakEventManager<AimTimersEventArgs>();

        public event EventHandler<AimTimersEventArgs> OnStatusChanged
        {
            add => _statusChangedEventManager.AddEventHandler(value);
            remove => _statusChangedEventManager.RemoveEventHandler(value);
        }

        private List<IAimTimer> _aimTimers = new List<IAimTimer>();

        public AimTimerNotificationService(ITimer timer)
        {
            _timer = timer;
        }

        public void Start()
        {
            _timer.Interval = 100;
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Elapsed -= OnTimedEvent;
            _timer.Enabled = false;
            _timer.Stop();
        }

        public void SetItemsToFollow(IEnumerable<IAimTimer> aimTimers)
        {
            lock (_lock)
            {
                _aimTimers = aimTimers.ToList();
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

                _statusChangedEventManager.HandleEvent(this, new AimTimersEventArgs { AimTimers = _aimTimers }, nameof(OnStatusChanged));
            }
        }

        public void Remove(IAimTimer aimTimer)
        {
            lock (_lock)
            {
                _aimTimers.Remove(aimTimer);
            }
        }
    }
}
