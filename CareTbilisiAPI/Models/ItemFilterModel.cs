using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public sealed class ItemFilterModel
    {
        public ProblemTypeEnum? Category { get; set; }

        public string? Location { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
