using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Services
{
    public interface IQueryService <TEntity>
    {
        ICollection<TEntity> GetAll();
        ICollection<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(string id);
    }
}
