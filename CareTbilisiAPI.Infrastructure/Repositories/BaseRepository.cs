using CareTbilisiAPI.Domain.Interfaces;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        private string CollectionName => $"{typeof(TEntity).Name.ToLower()}s";

        public BaseRepository(IDatabaseSettings databaseSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _collection = database.GetCollection<TEntity>(CollectionName);
        }

        public TEntity Create(TEntity entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public TEntity GetById(string id)
        {
            return _collection.Find(entity => entity.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _collection.DeleteOne(id);
        }

        public void Update(string id, TEntity entity)
        {
            _collection.ReplaceOne(entity => entity.Id == id, entity);
        }

        public ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return _collection.Find(predicate).ToList();
        }

        public ICollection<TEntity> GetAll()
        {
            return _collection.Find(entities => true).ToList();
        }

    }
}
