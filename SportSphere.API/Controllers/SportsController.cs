using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSphere.API.Services;
using SportSphere.Shared.DTOs;
using System.Threading.Tasks;

namespace SportSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly ISportService _sportService;

        public SportsController(ISportService sportService)
        {
            _sportService = sportService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSport(int id)
        {
            var sport = await _sportService.GetSportByIdAsync(id);
            if (sport == null)
                return NotFound();

            return Ok(sport);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSports()
        {
            var sports = await _sportService.GetAllSportsAsync();
            return Ok(sports);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSports([FromQuery] string searchTerm)
        {
            var sports = await _sportService.SearchSportsAsync(searchTerm);
            return Ok(sports);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSport([FromBody] SportCreateDto sportDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sportId = await _sportService.CreateSportAsync(sportDto);
            return CreatedAtAction(nameof(GetSport), new { id = sportId }, new { Id = sportId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSport(int id, [FromBody] SportUpdateDto sportDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sportService.UpdateSportAsync(id, sportDto);
            if (result)
                return Ok(new { Message = "Sport updated successfully" });

            return BadRequest(new { Message = "Failed to update sport" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSport(int id)
        {
            var result = await _sportService.DeleteSportAsync(id);
            if (result)
                return Ok(new { Message = "Sport deleted successfully" });

            return BadRequest(new { Message = "Failed to delete sport" });
        }

        [HttpGet("{sportId}/practitioners")]
        public async Task<IActionResult> GetSportPractitioners(int sportId)
        {
            var practitioners = await _sportService.GetSportPractitionersAsync(sportId);
            return Ok(practitioners);
        }

        [HttpGet("{sportId}/venues")]
        public async Task<IActionResult> GetSportVenues(int sportId)
        {
            var venues = await _sportService.GetSportVenuesAsync(sportId);
            return Ok(venues);
        }

        [HttpGet("{sportId}/events")]
        public async Task<IActionResult> GetSportEvents(int sportId)
        {
            var events = await _sportService.GetSportEventsAsync(sportId);
            return Ok(events);
        }
    }
} 