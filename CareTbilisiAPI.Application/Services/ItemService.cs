using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Interfaces.Services;
using CareTbilisiAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Application.Services
{
    public class ItemService : BaseService<Item, IItemRepository>
    {
        public ItemService(IItemRepository repository) : base(repository)
        {

        }
    }
}
