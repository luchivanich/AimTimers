namespace AimTimers.Services
{
    public interface IAimTimerTickService
    {
        void RefreshStatuses(System.Collections.Generic.List<Models.AimTimer> aimTimers);
        void RefreshTimer(Models.AimTimerInterval aimTimerItem);
        void ResumeTimer(Models.AimTimer aimTimer);
        void StartWatchingTimers();
        void StopTimer(Models.AimTimer aimTimer);
        void StopWatchingTimers();
    }
}
