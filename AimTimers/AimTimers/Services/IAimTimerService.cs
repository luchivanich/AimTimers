using System.Collections.Generic;
using System.Threading.Tasks;
using AimTimers.Models;

namespace AimTimers.Services
{
    public interface IAimTimerService
    {
        Task<IEnumerable<AimTimerItem>> GetActiveAimTimerItems();
    }
}
