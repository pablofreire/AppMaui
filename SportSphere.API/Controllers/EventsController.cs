using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSphere.API.Services;
using SportSphere.Shared.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);
            if (eventDto == null)
                return NotFound();

            return Ok(eventDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents([FromQuery] EventSearchDto searchDto)
        {
            var events = await _eventService.SearchEventsAsync(searchDto);
            return Ok(events);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyEvents([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusInKm = 10)
        {
            var events = await _eventService.GetNearbyEventsAsync(latitude, longitude, radiusInKm);
            return Ok(events);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var eventId = await _eventService.CreateEventAsync(userId, eventDto);
            return CreatedAtAction(nameof(GetEvent), new { id = eventId }, new { Id = eventId });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventUpdateDto eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _eventService.UpdateEventAsync(id, eventDto);
            if (result)
                return Ok(new { Message = "Event updated successfully" });

            return BadRequest(new { Message = "Failed to update event" });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (result)
                return Ok(new { Message = "Event deleted successfully" });

            return BadRequest(new { Message = "Failed to delete event" });
        }

        [Authorize]
        [HttpPost("{eventId}/join")]
        public async Task<IActionResult> JoinEvent(int eventId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var result = await _eventService.JoinEventAsync(eventId, userId);
            if (result)
                return Ok(new { Message = "Joined event successfully" });

            return BadRequest(new { Message = "Failed to join event" });
        }

        [Authorize]
        [HttpPost("{eventId}/leave")]
        public async Task<IActionResult> LeaveEvent(int eventId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var result = await _eventService.LeaveEventAsync(eventId, userId);
            if (result)
                return Ok(new { Message = "Left event successfully" });

            return BadRequest(new { Message = "Failed to leave event" });
        }

        [Authorize]
        [HttpPost("{eventId}/cancel")]
        public async Task<IActionResult> CancelEvent(int eventId)
        {
            var result = await _eventService.CancelEventAsync(eventId);
            if (result)
                return Ok(new { Message = "Event cancelled successfully" });

            return BadRequest(new { Message = "Failed to cancel event" });
        }

        [HttpGet("{eventId}/participants")]
        public async Task<IActionResult> GetEventParticipants(int eventId)
        {
            var participants = await _eventService.GetEventParticipantsAsync(eventId);
            return Ok(participants);
        }
    }
} 