using CareTbilisiAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CareTbilisiAPI.Models
{
    public class RequestItemModel
    {
        [Required, MaxLength(130, ErrorMessage = "Description max length is 130 characters")]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(100, ErrorMessage = "Location max length is 100 characters")]
        public string Location { get; set; } = string.Empty;

        [Required]
        public CityRegionEnum? CityRegion { get; set; }

        [Required]
        public ProblemTypeEnum? Category { get; set; }

        public ICollection<string> ? Comments { get; set; }
    }
}
