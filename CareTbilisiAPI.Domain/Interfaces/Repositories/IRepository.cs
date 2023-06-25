using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        ICollection<TEntity> GetAll();
        ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(string id);        
        TEntity Create(TEntity student);
        void Update(string id, TEntity student);
        void Remove(string id);
    }
}
