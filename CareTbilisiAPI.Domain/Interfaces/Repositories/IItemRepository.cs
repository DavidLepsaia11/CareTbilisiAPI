using CareTbilisiAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
         void UpdateByField(string id, Item entity);
    }
}
