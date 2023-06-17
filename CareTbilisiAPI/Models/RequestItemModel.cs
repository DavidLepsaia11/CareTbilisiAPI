using CareTbilisiAPI.Domain.Enums;

namespace CareTbilisiAPI.Models
{
    public class RequestItemModel
    {
        public string Description { get; set; } = null!;

        public byte[]  Picture { get; set; } = new byte[0];

        public string Location { get; set; } = null!;

        public StatusEnum? Status { get; set; }

        public ProblemTypeEnum? Category { get; set; }

        public ICollection<string> Comments { get; set; } = new List<string>();
    }
}
