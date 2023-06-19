using CareTbilisiAPI.Domain.Enums;
using CareTbilisiAPI.Domain.Interfaces;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(IDatabaseSettings databaseSettings, IMongoClient mongoClient) : base(databaseSettings, mongoClient)
        {

        }

        public IEnumerable<Item> FilterItemByAttribute(string? location , ProblemTypeEnum? category , DateTime? createDate, int currentPage , int pageSize)
        {
            Expression<Func<Item, bool>> predicate = (item) => (location != null ? item.Location == location : true ||
                                                               category != null ? item.Category == category : true ||
                                                               createDate != null ? item.CreateDate == createDate : true);

            var filteredItems = Filter(predicate)
                                .Skip((currentPage - 1) * pageSize)
                                .Take(pageSize);

            return filteredItems;
        }
    }
}
