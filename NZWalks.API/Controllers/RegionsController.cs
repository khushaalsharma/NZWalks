using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionsFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.DTOs;
using NZWalks.API.NewFolder.NewFolder;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    //http://localhost:5283/api/Regions -> will point to this controller
    [Route("api/[controller]")] //defines the route for this API 
    [ApiController] //tells the Application that this controller if for API use
    //[Authorize] //this ensures that only authenticated person/user can access this class, this was authorizing the whole class, to achieve role based authroization we need to add this before each method

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext; //able to use this to directly to access database
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger) //ctor shortcut
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //IActionResult return type is appropriate when multiple ActionResult return types are possible in an action
        [HttpGet] //as we are getting resource thus GET endpoint
        //[Authorize(Roles = "Reader")]


        //to convert a method into async first add async and Task<T> where T is return type
        public async Task<IActionResult> GetAll() //IActionResult is one of the controller action return type
        {
            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland Region",
            //        Code = "AKL",
            //        RegionImageUrl = "https://www.pexels.com/photo/new-zealand-city-view-19517609/"
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington Region",
            //        Code = "WLG",
            //        RegionImageUrl = "https://www.pexels.com/photo/wellington-cable-car-in-wellington-new-zealand-8379417/"
            //    },
            //}; JUST FOR EXAMPLE will DbContext to fetch data from Database now

            //var regionsDomain = dbContext.Regions.ToList(); //to read regions table in database and convert the data into a list
            //regions is currently a domain model object. we will convert it into a DTO
            //Map domain model to STO

            //var regionsDomain = await dbContext.Regions.ToListAsync();

            //var regionsDomain = await regionRepository.GetAllASync(); //abstraction successful

            //var regionsDto = new List<RegionDto>();
            //foreach(var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Name = regionDomain.Name,
            //        Code = regionDomain.Code,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //}

            //using mapper
            //var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //return Ok(regionsDto); //we are now exposing DTO instead of domain model which is said as the best practice

            var regionsDomain = await regionRepository.GetAllASync();

            logger.LogInformation($"Finished getting regions, : {regionsDomain}");

            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        // get single region or region by id
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        [Route("{id:Guid}")] //this makes sure that controller can read the parameter from the route, make sure that param in route is same as function
        public async Task<IActionResult> GetById(Guid id)
        {
            //var region = dbContext.Regions.Find(id); use this or
            //var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id); //this method is applicable to multiple data types whereas the Find is limited to List<T> and only the primary key
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomain = await regionRepository.GetById(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //DTO to domain model

            //var regionDto = new RegionDto
            //{
            //    Id=regionDomain.Id,
            //    Name = regionDomain.Name,   
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl,   
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomain);


            return Ok(regionDto);
        }

        //POST METHOD TO CREATE A REGION
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) //FromBody take sthe values from the body of the client
        {

            if(!ModelState.IsValid) //this checks if the addRegionRequestDto is according to the model constraints or model validation
            {
                return BadRequest(ModelState);
            }

            //map DTO to domain model 

            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            //};

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            regionDomainModel = await regionRepository.Create(regionDomainModel);
            
            //MAP Domain model back to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new {id = regionDomainModel.Id}, regionDto); //This function returns 200 response if an entry is made in the database
        }

        //PUT method to update the Region table
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel] //custom model validator 
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> updateById([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            //if(ModelState.IsValid == false)
            //{
            //    return BadRequest(ModelState);
            //} instead doing it here explicitly we can create custom model validation and invoke it before the function/method


            //Mapping the DTO to domain model for Repository use

            //var regionDomainModel = new Region
            //{
            //    Name = updateRegionRequestDto.Name,
            //    Code = updateRegionRequestDto.Code,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
            //};

            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            ////check if region exists
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            //if(regionDomainModel == null)
            //{
            //    return NotFound();
            //}

            ////Map to DTO TO domain model

            //regionDomainModel.Code = updateRegionRequestDto.Code;
            //regionDomainModel.Name = updateRegionRequestDto.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            //await dbContext.SaveChangesAsync(); //changes saved
            ////converting domain model to DTO
            // using the repository here

            regionDomainModel = await regionRepository.Update(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        //delete method
        [HttpDelete]
        //[Authorize(Roles = "Writer, Reader")]

        [Route("{id:Guid}")]
        public async Task<IActionResult> deleteById([FromRoute] Guid id)
        {
            //check if exists
            var regionDomainModel = await regionRepository.Delete(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            ////Deletiong
            //dbContext.Regions.Remove(regionDomainModel); //Remove doesn't have async version
            //await dbContext.SaveChangesAsync();

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
 