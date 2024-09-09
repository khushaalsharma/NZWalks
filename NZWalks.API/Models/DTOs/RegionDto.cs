namespace NZWalks.API.Models.DTOs
{
    public class RegionDto //this will have those properties that we want to expose to the client
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }


    }
}
