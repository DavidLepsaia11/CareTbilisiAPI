using CareTbilisiAPI.Domain.Enums;
using CareTbilisiAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Services
{
    public interface IItemService : ICommandService<Item> , IQueryService<Item>
    {
        IEnumerable<Item> FilterItemByAttribute(string? location, ProblemTypeEnum? category, DateTime? createDate, int currentPage = 1, int pageSize = 6);
    }
}
