using CareTbilisiAPI.Domain.Interfaces;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Application.Services
{
    public class BaseService<TEntity , TRepository> : IQueryService<TEntity>, ICommandService<TEntity>
        where TEntity : IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;

        public BaseService(TRepository repository)
        {
            _repository = repository;
        }

        public TEntity Create(TEntity student)
        {
           return _repository.Create(student);
        }

        public ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.Filter(predicate);
        }

        public ICollection<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public TEntity GetById(string id)
        {
            return _repository.GetById(id);
        }

        public void Remove(string id)
        {
            _repository.Remove(id);
        }

        public void Update(string id, TEntity student)
        {
           _repository.Update(id, student);
        }
    }
}
