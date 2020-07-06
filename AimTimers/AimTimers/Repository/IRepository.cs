using System.Collections.Generic;
using AimTimers.Models;

namespace AimTimers.Repository
{
    public interface IRepository
    {
        void Save<T>(T model, string id);
        void Save<T>(T model) where T : IModel;
        List<T> LoadAll<T>() where T : IModel;

        void Delete(string id);
    }
}
