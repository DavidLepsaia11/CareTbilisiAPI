using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public class ResponseItemModel
    {
        public string Id { get; init; } 

        public string Description { get; init; } = null!;

        public string Location { get; init; } = null!;

        public string? CityRegion { get; init; }

        public string? Category { get; init; }

        public string? Status { get; init; }

        public ICollection<string>? Comments { get; init; }
    }
}
