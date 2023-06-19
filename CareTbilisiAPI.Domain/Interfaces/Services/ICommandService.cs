using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Services
{
    public interface ICommandService<TEntity>
    {
        TEntity Create(TEntity student);
        void Update(string id, TEntity student);
        void Remove(string id);
    }
}
