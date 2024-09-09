namespace NZWalks.API.NewFolder.NewFolder
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; } //adding ? makes this data member as nullable in database
    }
}
