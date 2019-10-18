using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AimTimers.Models;

namespace AimTimers.Services
{
    public class MockDataStore : IDataStore<AimTimerItem>
    {
        List<AimTimer> aimTimers;
        List<AimTimerItem> aimTimerItems;

        public MockDataStore()
        {
            aimTimers = new List<AimTimer>
            {
                new AimTimer { Id = Guid.NewGuid().ToString(), Title = "First item", Description="This is an item description.", Time = new TimeSpan(0, 10,0 ) },
                new AimTimer { Id = Guid.NewGuid().ToString(), Title = "Second item", Description="This is an item description.", Time = new TimeSpan(0, 1,0 ) },
                new AimTimer { Id = Guid.NewGuid().ToString(), Title = "Third item", Description="This is an item description.", Time = new TimeSpan(1, 0, 3 ) },
                new AimTimer { Id = Guid.NewGuid().ToString(), Title = "Fourth item", Description="This is an item description.", Time = new TimeSpan(0, 0, 1 ) },
                new AimTimer { Id = Guid.NewGuid().ToString(), Title = "Fifth item", Description="This is an item description.", Time = new TimeSpan(0, 0, 5 ) },
                new AimTimer { Id = Guid.NewGuid().ToString(), Title = "Sixth item", Description="This is an item description.", Time = new TimeSpan(0, 0, 10 ) },
            };

            aimTimerItems = new List<AimTimerItem>()
            {
                new AimTimerItem {
                    Id = 1,
                    AimTimer = aimTimers[0],
                    AimTimerIntervals = new List<AimTimerInterval>
                    {
                        new AimTimerInterval
                        {
                            Id = "1",
                            AimTimerItem = null,
                            StartDate = new DateTime(2019, 08, 1, 10, 0, 0),
                            EndDate = new DateTime(2019, 08, 1, 10, 4, 30)
                        },
                        new AimTimerInterval
                        {
                            Id = "1",
                            AimTimerItem = null,
                            StartDate = DateTime.Now.AddMinutes(-1),
                            EndDate = null
                        }
                    }
                },
                new AimTimerItem
                {
                    Id = 2,
                    AimTimer = aimTimers[1]
                }
            };
        }

        public async Task<bool> AddItemAsync(AimTimerItem item)
        {
            aimTimerItems.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(AimTimerItem item)
        {
            var oldItem = aimTimerItems.Where((AimTimerItem arg) => arg.Id == item.Id).FirstOrDefault();
            aimTimerItems.Remove(oldItem);
            aimTimerItems.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = aimTimerItems.Where((AimTimerItem arg) => arg.Id.ToString() == id).FirstOrDefault();
            aimTimerItems.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<AimTimerItem> GetItemAsync(string id)
        {
            return await Task.FromResult(aimTimerItems.FirstOrDefault(s => s.Id.ToString() == id));
        }

        public async Task<IEnumerable<AimTimerItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(aimTimerItems);
        }
    }
}