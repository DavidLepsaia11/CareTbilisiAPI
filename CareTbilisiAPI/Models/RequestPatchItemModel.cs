namespace CareTbilisiAPI.Models
{
    public class RequestPatchItemModel
    {
        public string? Description { get; set; } 

        public string? Location { get; set; } 

        public string? CityRegion { get; set; }

        public string? Category { get; set; }

        public ICollection<string>? Comments { get; set; }
    }
}
