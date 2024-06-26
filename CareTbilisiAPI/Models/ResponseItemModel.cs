﻿using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public class ResponseItemModel
    {
        public string Id { get; init; } 

        public string Description { get; init; } = null!;

        public string Location { get; init; } = null!;

        public CityRegionEnum CityRegion { get; init; }

        public ProblemTypeEnum Category { get; init; }

        public StatusEnum? Status { get; init; }

        public ICollection<string>? Comments { get; init; }
    }
}
