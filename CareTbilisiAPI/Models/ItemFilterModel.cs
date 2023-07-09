using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public sealed class ItemFilterModel
    {
        public string Category { get; set; } = string.Empty;

        public string CityRegion { get; set; } = string.Empty;

        public DateTime? CreateDate { get; set; }
    }
}
