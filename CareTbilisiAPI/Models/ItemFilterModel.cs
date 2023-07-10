using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public sealed class ItemFilterModel
    {
        public string? Category { get; set; } 

        public string? CityRegion { get; set; } 

        public DateTime? CreateDate { get; set; }
    }
}
