using System;
using System.Collections.Generic;
using AimTimers.Bl;

namespace AimTimers.Services
{
    public interface IAimTimerNotificationService
    {
        event EventHandler<AimTimersEventArgs> OnStatusChanged;

        void SetItemsToFollow(IEnumerable<IAimTimerItem> aimTimerItems);
        void Start();
        void Stop();
        void Remove(IAimTimerItem aimTimerItem);
    }
}
