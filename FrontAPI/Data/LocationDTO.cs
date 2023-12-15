namespace FrontAPI.Data
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ZipCode { get; set; }
        public string StreetAddress { get; set; } = null!;
        public int StateId { get; set; }
        public string? StateName { get; set; } = null!;
    }
}
