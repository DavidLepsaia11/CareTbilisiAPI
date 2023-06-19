﻿using CareTbilisiAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareTbilisiAPI.Domain.Interfaces.Services
{
    public interface IItemService : ICommandService<Item> , IQueryService<Item>
    {

    }
}