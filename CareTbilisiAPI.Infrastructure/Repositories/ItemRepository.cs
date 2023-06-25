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
            Expression<Func<Item, bool>> predicate;

            if (( !string.IsNullOrEmpty(location) && category == null && createDate.Value == DateTime.MinValue) ||
                  (category != null && string.IsNullOrEmpty(location) && createDate.Value == DateTime.MinValue) ||
                  (createDate.Value == DateTime.MinValue && category == null && string.IsNullOrEmpty(location))
                )
            {
                predicate = (item) => (item.Category == category ||
                                       item.Location == location ||
                                       item.CreateDate.Year == createDate.Value.Year &&
                                       item.CreateDate.Month == createDate.Value.Month &&
                                       item.CreateDate.Day == createDate.Value.Day
                                                                                    );
            }

            else if (!string.IsNullOrEmpty(location) && category != null && createDate.Value == DateTime.MinValue)
            {
                predicate = (item) => (item.Category == category &&
                                       item.Location == location );
            }

            else if (string.IsNullOrEmpty(location) && category != null && createDate.Value != DateTime.MinValue)
            {
                predicate = (item) => (item.Category == category &&
                                       item.CreateDate.Year == createDate.Value.Year &&
                                       item.CreateDate.Month == createDate.Value.Month &&
                                       item.CreateDate.Day == createDate.Value.Day
                                        );
            }

            else if (!string.IsNullOrEmpty(location) && category == null && createDate.Value != DateTime.MinValue)
            {
                predicate = (item) => (item.Location == location &&
                                      item.CreateDate.Year == createDate.Value.Year &&
                                      item.CreateDate.Month == createDate.Value.Month &&
                                      item.CreateDate.Day == createDate.Value.Day
                                       );
            }

            else 
            {
                predicate = (item) => (item.Category == category &&
                                       item.Location == location &&
                                       item.CreateDate.Year == createDate.Value.Year &&
                                       item.CreateDate.Month == createDate.Value.Month &&
                                       item.CreateDate.Day == createDate.Value.Day
                                                                                    );
            }


            var filteredItems = Filter(predicate)
                                .Skip((currentPage - 1) * pageSize)
                                .Take(pageSize);

            return filteredItems;
        }

        public IEnumerable<Item> SortItemDescByCreateDay(int currentPage, int pageSize)
        {
            var items = GetAll()
                 .OrderByDescending( item => item.CreateDate)
                 .Skip((currentPage - 1) * pageSize)
                 .Take(pageSize);


            return items;
        }
    }
}
