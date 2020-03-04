using System.Collections.Generic;
using AimTimers.Models;

namespace AimTimers.Repository
{
    public interface IRepository
    {
        void Save<T>(T model, string id);
        List<T> LoadAll<T>() where T : IModel;

        void Delete(string id);
    }
}
