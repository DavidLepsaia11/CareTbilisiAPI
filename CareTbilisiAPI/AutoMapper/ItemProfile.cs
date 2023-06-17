using AutoMapper;
using CareTbilisiAPI.Domain.Models;
using CareTbilisiAPI.Models;

namespace CareTbilisiAPI.AutoMapper
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<RequestItemModel,Item>();
            CreateMap<Item,ResponseItemModel>();
        }
    }
}
