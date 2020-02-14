using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Repository;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private readonly IRepository _repository;

        private object _lock = new object();
        private IEnumerable<AimTimerModel> _aimTimers = new List<AimTimerModel>();
                
        public AimTimerService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<IAimTimer> GetActiveAimTimers()
        {
            lock (_lock)
            {
                _aimTimers = _repository.LoadAll<AimTimerModel>();
            }
            return _aimTimers.Select(i => new AimTimer(i)).ToList();
        }

        public void AddAimTimer(AimTimerModel aimTimer)
        {
            if (string.IsNullOrEmpty(aimTimer.Id))
            {
                aimTimer.Id = Guid.NewGuid().ToString();
            }
            _repository.Save(aimTimer, aimTimer.Id);
        }
    }
}
