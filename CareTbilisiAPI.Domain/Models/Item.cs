using CareTbilisiAPI.Domain.Enums;
using CareTbilisiAPI.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Models
{
    [BsonIgnoreExtraElements]
    public class Item : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("picturePath")]
        public string? PicturePath { get; set; }

        [BsonElement("status")]
        public StatusEnum? Status { get; set; }

        [BsonElement("category")]
        public ProblemTypeEnum? Category { get; set; }

        [BsonElement("cityRegion")]
        public CityRegionEnum? CityRegion { get; set; }

        [BsonElement("comments")]
        public ICollection<string> Comments { get; set; }

        [BsonElement("createDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("updateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}
