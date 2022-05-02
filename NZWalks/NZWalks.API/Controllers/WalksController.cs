using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this._walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            // Fetch data from database
            var walks = await _walkRepository.GetAllAsync();

            // Transform to DTO

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            // Return response

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAsync")]

        public async Task<IActionResult> GetAsync(Guid id)
        {
            // Fetch data from database
            var walk = await _walkRepository.GetAsync(id);

            // Transform to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {

            // Convert request DTO to Domain object

            var walkDomain = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass domain object to the the repository

            walkDomain = await _walkRepository.AddAsync(walkDomain);

            // Convert the domain back to DTO

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Send DTO response back to Client

            return CreatedAtAction(nameof(GetAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to domain object

            var walkDomain = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
                RegionId = updateWalkRequest.RegionId
            };

            // Pass the details to the repository

            walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            // Handle Null (not found)

            if (walkDomain == null)
            {
                return NotFound();

            }

            // Convert back Domain to DTO

            var walkDTO = new Models.DTO.Walk
            {
                Id = id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Return response

            return Ok(walkDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var walkDomain = await _walkRepository.DeleteAsync(id);

            if(walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        } 
    }
}


