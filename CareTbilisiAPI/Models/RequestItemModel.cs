using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public class RequestItemModel
    {
        public string? Description { get; set; }

        public string? Location { get; set; }

        public CityRegionEnum? CityRegion { get; set; }

        public StatusEnum? Status { get; set; }

        public ProblemTypeEnum? Category { get; set; }

        public ICollection<string> ? Comments { get; set; }
    }
}
