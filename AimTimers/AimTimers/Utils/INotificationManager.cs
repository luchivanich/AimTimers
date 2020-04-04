using System;

namespace AimTimers.Utils
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        int ScheduleNotification(string title, string message);
        void ReceiveNotification(string title, string message);
    }
}
