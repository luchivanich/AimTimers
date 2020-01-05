using System.Collections.Generic;

namespace AimTimers.Repository
{
    public interface IRepository
    {
        void Save<T>(T model, string id);
        List<T> LoadAll<T>();
    }
}
