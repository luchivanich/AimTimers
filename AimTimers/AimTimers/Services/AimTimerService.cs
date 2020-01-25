using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Models;
using AimTimers.Repository;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private readonly IRepository _repository;

        private object _lock = new object();
        private IEnumerable<AimTimer> _aimTimers = new List<AimTimer>();
                
        public AimTimerService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AimTimer> GetActiveAimTimers()
        {
            lock (_lock)
            {
                _aimTimers = _repository.LoadAll<AimTimer>();
            }
            return _aimTimers;
        }

        public void AddAimTimer(AimTimer aimTimer)
        {
            if (string.IsNullOrEmpty(aimTimer.Id))
            {
                aimTimer.Id = Guid.NewGuid().ToString();
            }
            _repository.Save(aimTimer, aimTimer.Id);
        }

        public void Stop(AimTimer aimTimer)
        {
            var lastInterval = aimTimer.GetAimTimerByDate(DateTime.Now).AimTimerIntervals.SingleOrDefault(i => i.EndDate == null);
            if (lastInterval == null)
            {
                return;
            }

            lastInterval.EndDate = DateTime.Now;
        }

        public void Start(AimTimer aimTimer)
        {
            var now = DateTime.Now;
            var currentAimTimerItem = aimTimer.GetAimTimerByDate(now);
            if (now < currentAimTimerItem.StartOfActivityPeriod || now > currentAimTimerItem.EndOfActivityPeriod || currentAimTimerItem.AimTimerIntervals.Any(i => i.EndDate == null))
            {
                return;
            }

            currentAimTimerItem.AimTimerIntervals.Add(new AimTimerInterval { StartDate = now, EndDate = null });
        }
    }
}
