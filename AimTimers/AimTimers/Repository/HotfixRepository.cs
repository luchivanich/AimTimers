namespace AimTimers.Repository
{
    public class HotfixRepository : BaseRepository, IRepository
    {
        protected override string GetRepositoryName()
        {
            return "Hotfix";
        }
    }
}
