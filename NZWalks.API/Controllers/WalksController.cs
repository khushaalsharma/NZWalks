using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionsFilters;
using NZWalks.API.Models.DTOs;
using NZWalks.API.NewFolder.NewFolder;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create Walk method
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //map DTO to domain model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Get all teh walks
        //GET : /api/walks?filterOn=Name&filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000) //will be adding filtering, sorting and pagination paramters here
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,  isAscending, pageNumber, pageSize);

            //custom exceeption for gloabl handler test
            throw new Exception("Something wen wrong");

            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        //Get Walk data from an ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetById(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Updating the Walk 
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]

        public async Task<IActionResult> updateWalk([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto walkDTO)
        {
            var walkDomainModel = mapper.Map<Walk>(walkDTO);

            walkDomainModel = await walkRepository.UpdateWalk(id, walkDomainModel);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomainModel)); 
        }

        //delete walk 

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> deleteWalk([FromRoute]Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteWalk(id);
            
            if(walkDomainModel == null) //doesn't exist
            {
                return NotFound();

            }

            //deleted the walk
            //mapping to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
