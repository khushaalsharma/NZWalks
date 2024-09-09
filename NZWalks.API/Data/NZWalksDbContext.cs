using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.NewFolder.NewFolder;
/*
 n C#, a DbContext instance represents a session with a database and can be used to query and save entity instances. 
It's a combination of the Unit of Work and Repository patterns, and is similar to ObjectContext. The DbContext class 
in EF Core encapsulates database logic within the application, making it easier to work with the database and maintain 
code reusability and separation of concerns.
 */
namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; } //prop shortcut
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; } 

        //came from writing -> override OnModelCreating [enter]
        protected override void OnModelCreating(ModelBuilder modelBuilder) //to add this to the database create another migration
        {
            base.OnModelCreating(modelBuilder);

            //data seeding for Difficulties
            //easy medium and hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty
                {
                    Id =Guid.Parse("41bf73e5-5fae-457f-b837-4b7c95534aff") , //we can't use Guid.newGuid here as it will change everytime the application is run thus we use C# Interactive Window in View Tab to generate a few Guid ids
                    Name = "Easy"
                },
                new Difficulty
                {
                    Id = Guid.Parse("8796e690-94f8-4c1b-b507-29fa58b6f695"),
                    Name = "Medium"
                },
                new Difficulty
                {
                    Id = Guid.Parse("125dba1d-27cc-4793-8052-de5358004b37"),
                    Name = "Hard"
                },
            };
            //seed this data to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //seeding data for Regions
            var regions = new List<Region>() 
            {
                new Region
                {
                    Id = Guid.Parse("9adaae02-3272-4cc5-b301-efbbe092b3a4"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://www.pexels.com/photo/new-zealand-city-view-19517609/"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            //seed regions into the database
            modelBuilder.Entity<Region>().HasData(regions);


        }
    }
}
