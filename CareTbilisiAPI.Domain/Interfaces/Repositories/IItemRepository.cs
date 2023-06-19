using CareTbilisiAPI.Domain.Enums;
using CareTbilisiAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Repositories 
{
    public interface IItemRepository : IRepository<Item>
    {
        IEnumerable<Item> FilterItemByAttribute(string? location, ProblemTypeEnum? category, DateTime? createDate, int currentPage, int pageSize);
    }
}
