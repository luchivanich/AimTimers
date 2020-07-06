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

        private List<IAimTimerItem> _aimTimerItems = new List<IAimTimerItem>();

        public AimTimerNotificationService(ITimer timer)
        {
            _timer = timer;
        }

        public void Start()
        {
            _timer.Interval = 1000;
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

        public void SetItemsToFollow(IEnumerable<IAimTimerItem> aimTimerItems)
        {
            lock (_lock)
            {
                _aimTimerItems = aimTimerItems.ToList();
            }
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                if (_aimTimerItems == null || _aimTimerItems.Count() == 0)
                {
                    return;
                }

                _statusChangedEventManager.HandleEvent(this, new AimTimersEventArgs { AimTimerItems = _aimTimerItems }, nameof(OnStatusChanged));
            }
        }

        public void Remove(IAimTimerItem aimTimerItem)
        {
            lock (_lock)
            {
                _aimTimerItems.Remove(aimTimerItem);
            }
        }
    }
}
