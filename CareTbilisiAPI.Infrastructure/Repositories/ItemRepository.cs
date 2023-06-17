using CareTbilisiAPI.Domain.Interfaces;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(IDatabaseSettings databaseSettings, IMongoClient mongoClient) : base(databaseSettings, mongoClient)
        {

        }

        public void UpdateByField(string id, Item item)
        {
            var itemForUpdate = Builders<Item>.Update.Set("updateDate", item.UpdateDate);

            if (!string.IsNullOrEmpty(item.Description))   itemForUpdate.Set("description", item.Description);
         
            if (!string.IsNullOrEmpty(item.Location)) itemForUpdate.Set("location", item.Location);

            if (item.Category != null) itemForUpdate.Set("category", item.Category);
           
            if (item.Status != null) itemForUpdate.Set("status", item.Status);

            if (item.Comments.Count() != 0) itemForUpdate.PushEach("comments", item.Comments);

            if (item.Picture.Count() != 0) itemForUpdate.PushEach("picture", item.Picture);

            _collection.UpdateOne(entity => entity.Id == id, itemForUpdate);
        }
    }
}
