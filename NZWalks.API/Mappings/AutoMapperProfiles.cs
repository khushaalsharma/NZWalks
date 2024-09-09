using AutoMapper;
using NZWalks.API.Models.DTOs;
using NZWalks.API.NewFolder.NewFolder;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile //Profile is within auto mapper package
    {
        //public AutoMapperProfiles()
        //{
        //    CreateMap<UserDTO, UserDomain>().ReverseMap();
        //    //using reverse map we create an auto map in both directions

        //    //if the properties name is not same for src and dest
        //    CreateMap<UserDTO, UserDomain>()
        //        .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FullName))
        //        .ReverseMap();
        //}

        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap(); //need to inject this automapper in the Program.cs file
            CreateMap<Walk, AddWalkRequestDto>().ReverseMap();  
            CreateMap<Region, AddRegionRequestDto>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDto>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
        }
    }

    //public class UserDTO
    //{
    //    public string FullName { get; set; }
    //}

    //public class UserDomain
    //{
    //    public string Name { get; set; }
    //}
}
