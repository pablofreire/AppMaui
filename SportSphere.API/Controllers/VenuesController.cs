using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSphere.API.Services;
using SportSphere.Shared.DTOs;
using System.Threading.Tasks;

namespace SportSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenue(int id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);
            if (venue == null)
                return NotFound();

            return Ok(venue);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await _venueService.GetAllVenuesAsync();
            return Ok(venues);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVenues([FromQuery] string searchTerm)
        {
            var venues = await _venueService.SearchVenuesAsync(searchTerm);
            return Ok(venues);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyVenues([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusInKm = 10)
        {
            var venues = await _venueService.GetVenuesByLocationAsync(latitude, longitude, radiusInKm);
            return Ok(venues);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateVenue([FromBody] VenueCreateDto venueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var venueId = await _venueService.CreateVenueAsync(venueDto);
            return CreatedAtAction(nameof(GetVenue), new { id = venueId }, new { Id = venueId });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVenue(int id, [FromBody] VenueUpdateDto venueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _venueService.UpdateVenueAsync(id, venueDto);
            if (result)
                return Ok(new { Message = "Venue updated successfully" });

            return BadRequest(new { Message = "Failed to update venue" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            var result = await _venueService.DeleteVenueAsync(id);
            if (result)
                return Ok(new { Message = "Venue deleted successfully" });

            return BadRequest(new { Message = "Failed to delete venue" });
        }

        [HttpGet("{venueId}/sports")]
        public async Task<IActionResult> GetVenueSports(int venueId)
        {
            var sports = await _venueService.GetVenueSportsAsync(venueId);
            return Ok(sports);
        }

        [HttpGet("{venueId}/events")]
        public async Task<IActionResult> GetVenueEvents(int venueId)
        {
            var events = await _venueService.GetVenueEventsAsync(venueId);
            return Ok(events);
        }
    }
} 