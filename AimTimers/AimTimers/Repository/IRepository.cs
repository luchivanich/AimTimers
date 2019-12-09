namespace AimTimers.Repository
{
    public interface IRepository
    {
        void Save<T>(T model);
    }
}
