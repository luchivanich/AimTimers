using System;
using System.IO;
using Couchbase.Lite;

namespace AimTimers.Repository
{
    public class BaseRepository : IDisposable, IRepository
    {
        DatabaseConfiguration _databaseConfig;
        protected DatabaseConfiguration DatabaseConfig
        {
            get
            {
                if (_databaseConfig == null)
                {
                    _databaseConfig = new DatabaseConfiguration
                    {
                        Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AimTimer")
                    };
                }

                return _databaseConfig;
            }
            set => _databaseConfig = value;
        }

        Database _database;
        protected Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database("AimTimer", DatabaseConfig);
                }

                return _database;
            }
            private set => _database = value;
        }

        public void Dispose()
        {
            DatabaseConfig = null;
            Database.Close();
            Database = null;
        }

        public void Save<T>(T model)
        {
            var mutableDocument = model.ToMutableDocument();
            Database.Save(mutableDocument);
        }
    }
}
