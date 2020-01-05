using System;
using System.Collections.Generic;
using System.IO;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using Newtonsoft.Json;

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

        public void Save<T>(T model, string id)
        {
            try
            {
                var mutableDocument = model.ToMutableDocument(id);
                Database.Save(mutableDocument);
            }
            catch(Exception e)
            {

            }
        }

        public List<T> LoadAll<T>()
        {
            var result = new List<T>();
            using (var query = QueryBuilder.Select(SelectResult.All()).From(DataSource.Database(Database)))
            {
                foreach (var item in query.Execute())
                {
                    try
                    {
                        var body = item.GetDictionary(0);
                        var jsonString = JsonConvert.SerializeObject(body);
                    
                        result.Add(JsonConvert.DeserializeObject<T>(jsonString));
                    }
                    catch(Exception e)
                    {

                    }
                }
            }
            return result;
        }
    }
}
