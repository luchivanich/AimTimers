using System.Timers;

namespace AimTimers.Utils
{
    public interface ITimer
    {
        void Start();
        void Stop();
        bool Enabled { get; set; }
        double Interval { get; set; }

        event ElapsedEventHandler Elapsed;
    }
}
