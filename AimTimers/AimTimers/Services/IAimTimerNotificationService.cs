using System;

namespace AimTimers.Services
{
    public interface IAimTimerNotificationService
    {
        event EventHandler<AimTimersEventArgs> OnStatusChanged;

        void Start();
        void Stop();
    }
}
