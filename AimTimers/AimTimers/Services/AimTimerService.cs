using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AimTimers.Models;

namespace AimTimers.Services
{
    public class AimTimerService : IAimTimerService
    {
        private readonly IDataStore<AimTimerItem> _dataStore;

        public AimTimerService(IDataStore<AimTimerItem> dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<IEnumerable<AimTimerItem>> GetActiveAimTimerItems()
        {
            return await _dataStore.GetItemsAsync();
        }
    }
}
