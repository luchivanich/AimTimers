using System;
using System.Collections.Generic;
using AimTimers.Models;
using AimTimers.Repository;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private readonly IRepository _repository;

        private List<AimTimer> _aimTimers = new List<AimTimer>();

        public AimTimerService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AimTimer> GetActiveAimTimers()
        {
            return _repository.LoadAll<AimTimer>();
        }

        public void AddAimTimer(AimTimer aimTimer)
        {
            if (string.IsNullOrEmpty(aimTimer.Id))
            {
                aimTimer.Id = Guid.NewGuid().ToString();
            }
            _repository.Save(aimTimer, aimTimer.Id);
        }
    }
}
