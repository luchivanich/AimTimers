using System;
using System.Collections.Generic;
using System.IO;
using AimTimers.Models;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using Newtonsoft.Json;

namespace AimTimers.Repository
{
    public abstract class BaseRepository : IDisposable, IRepository
    {
        private const string TYPE_PROPERTY = "type";
        protected abstract string GetRepositoryName();

        DatabaseConfiguration _databaseConfig;
        protected DatabaseConfiguration DatabaseConfig
        {
            get
            {
                if (_databaseConfig == null)
                {
                    _databaseConfig = new DatabaseConfiguration
                    {
                        Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GetRepositoryName())
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
                    _database = new Database(GetRepositoryName(), DatabaseConfig);
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
            using (var modelAsDocument = model.ToMutableDocument(id))
            {
                modelAsDocument.SetString(TYPE_PROPERTY, typeof(T).Name);
                Database.Save(modelAsDocument);
            }
        }

        public void Save<T>(T model) where T : IModel
        {
            Save(model, model.Id);
        }

        public List<T> LoadAll<T>() where T: IModel
        {
            var result = new List<T>();
            using (var query = QueryBuilder
                .Select(SelectResult.All(), SelectResult.Expression(Meta.ID))
                .From(DataSource.Database(Database))
                .Where(Expression.Property(TYPE_PROPERTY).EqualTo(Expression.String(typeof(T).Name))))
            {
                foreach (var item in query.Execute())
                {
                    var body = item.GetDictionary(0);
                    var jsonString = JsonConvert.SerializeObject(body);
                    var resultItem = JsonConvert.DeserializeObject<T>(jsonString);
                    resultItem.Id = item[1].String;
                    result.Add(resultItem);
                }
            }
            return result;
        }

        public List<T> LoadAllByKey<T>(string key, object value) where T: IModel
        {
            var result = new List<T>();
            using (var query = QueryBuilder
                .Select(SelectResult.All(), SelectResult.Expression(Meta.ID))
                .From(DataSource.Database(Database))
                .Where(
                    Expression.Property(TYPE_PROPERTY).EqualTo(Expression.String(typeof(T).Name))
                    .And(Expression.Property(key).EqualTo(Expression.Value(value))))
                )
            {
                foreach (var item in query.Execute())
                {
                    var body = item.GetDictionary(0);
                    var jsonString = JsonConvert.SerializeObject(body);
                    var resultItem = JsonConvert.DeserializeObject<T>(jsonString);
                    resultItem.Id = item[1].String;
                    result.Add(resultItem);
                }
            }
            return result;
        }

        public void Delete(string id)
        {
            var documentToDelete = Database.GetDocument(id);
            if (documentToDelete == null)
            {
                return;
            }
            Database.Delete(documentToDelete);
        }
    }
}
