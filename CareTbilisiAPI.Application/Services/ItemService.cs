using CareTbilisiAPI.Domain.Enums;
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
    public class ItemService : BaseService<Item, IItemRepository> , IItemService
    {
        public ItemService(IItemRepository repository) : base(repository)
        {

        }

        public bool Exist(string id)
        {
            return _repository.Exist(id);
        }

        public IEnumerable<Item> FilterItemByAttribute(string? location, ProblemTypeEnum? category, DateTime? createDate, int currentPage, int pageSize)
        {
            return _repository.FilterItemByAttribute(location, category, createDate, currentPage, pageSize);
        }

        public IEnumerable<Item> SortItemDescByCreateDay(int currentPage, int pageSize)
        {
            return _repository.SortItemDescByCreateDay(currentPage, pageSize);
        }
    }
}
