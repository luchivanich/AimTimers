using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using AimTimers.Models;

namespace AimTimers.Services
{
    public class AimTimerTickService : IAimTimerTickService, IDisposable
    {
        private Timer _timer;
        private IDataStore<AimTimer> _dataStore;

        private List<AimTimerInterval> _aimTimerItems = new List<AimTimerInterval>();

        public AimTimerTickService()
        {

        }

        public AimTimerTickService(IDataStore<AimTimer> dataStore)
        {
            _dataStore = dataStore;
        }

        public void RefreshStatuses(List<AimTimer> aimTimers)
        {
            //foreach (var aimTimer in aimTimers)
            //{
            //    var aimTimerItem = GetOrCreateAimTimerItemIfNecessary(aimTimer);
            //    if (aimTimerItem.Status != AimTimerItemStatus.Resumed)
            //    {
            //        continue;
            //    }

            //    RefreshTimer(aimTimerItem);
            //    if (aimTimerItem.Status == AimTimerItemStatus.Resumed)
            //    {
            //        ResumeTimer(aimTimer);
            //    }
            //}
        }

        public void StartWatchingTimers()
        {
            //_timer = new Timer();
            //_timer.Interval = 1000;
            //_timer.Elapsed += OnTimedEvent;

            //_timer.Enabled = true;
        }

        private int GetInterval()
        {
            return 1;
        }

        public void StopWatchingTimers()
        {
            //_timer.Stop();
            //_timer.Dispose();
        }

        public void ResumeTimer(AimTimer aimTimer)
        {
            //lock (_aimTimerItems)
            //{
            //    var aimTimerItem = GetOrCreateAimTimerItemIfNecessary(aimTimer);
            //    aimTimerItem.LastTimeRefreshed = DateTime.Now;
            //    aimTimerItem.Status = AimTimerItemStatus.Resumed;
            //    _aimTimerItems.Add(aimTimerItem);
            //}
        }

        public void StopTimer(AimTimer aimTimer)
        {
            //lock (_aimTimerItems)
            //{
            //    var aimTimerItem = GetOrCreateAimTimerItemIfNecessary(aimTimer);
            //    aimTimerItem.Status = AimTimerItemStatus.Stoped;
            //    _aimTimerItems.Remove(aimTimerItem);
            //}
        }

        private AimTimerInterval GetOrCreateAimTimerItemIfNecessary(AimTimer aimTimer)
        {
            //if (aimTimer.AimTimerItems.All(i => i.StartDate.Date != DateTime.Today))
            //{
            //    aimTimer.AimTimerItems.Add(new AimTimerInterval
            //    {
            //        AimTimerId = aimTimer.Id,
            //        StartDate = DateTime.Today,
            //        Status = AimTimerItemStatus.Resumed, //AimTimerItemStatus.Stoped,
            //        TimeLeft = aimTimer.Time
            //    });
            //}


            //return aimTimer.AimTimerItems.SingleOrDefault(i => i.StartDate.Date == DateTime.Today);
            return null;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            //lock (_aimTimerItems)
            //{
            //    foreach (var item in _aimTimerItems)
            //    {
            //        RefreshTimer(item);
            //    }
            //}
        }

        public void RefreshTimer(AimTimerInterval aimTimerItem)
        {
            //var now = DateTime.Now;
            //aimTimerItem.TimeLeft = aimTimerItem.TimeLeft.Subtract(now - aimTimerItem.LastTimeRefreshed);
            //aimTimerItem.LastTimeRefreshed = now;
            //if (TimeSpan.Compare(aimTimerItem.TimeLeft, TimeSpan.Zero) <= 0)
            //{
            //    aimTimerItem.TimeLeft = TimeSpan.Zero;
            //    aimTimerItem.Status = AimTimerItemStatus.Finished;
            //}
        }

        public void Dispose()
        {
            StopWatchingTimers();
        }
    }
}
