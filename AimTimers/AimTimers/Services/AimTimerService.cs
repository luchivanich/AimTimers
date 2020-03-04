using System;
using System.Collections.Generic;
using AimTimers.Models;
using AimTimers.Repository;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private readonly IRepository _repository;

        private IEnumerable<AimTimerModel> _aimTimers = new List<AimTimerModel>();
                
        public AimTimerService(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AimTimerModel> GetActiveAimTimers()
        {
            return _repository.LoadAll<AimTimerModel>();
        }

        public void AddAimTimer(AimTimerModel aimTimer)
        {
            if (string.IsNullOrEmpty(aimTimer.Id))
            {
                aimTimer.Id = Guid.NewGuid().ToString();
            }
            _repository.Save(aimTimer, aimTimer.Id);
        }

        public void DeleteAimTimer(string aimTimerId)
        {
            _repository.Delete(aimTimerId);
        }
    }
}
