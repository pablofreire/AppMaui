using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSphere.API.Services;
using SportSphere.Shared.DTOs;
using System.Threading.Tasks;

namespace SportSphere.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
        {
            var users = await _userService.SearchUsersAsync(searchTerm);
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(id, userDto);
            if (result)
                return Ok(new { Message = "User updated successfully" });

            return BadRequest(new { Message = "Failed to update user" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result)
                return Ok(new { Message = "User deleted successfully" });

            return BadRequest(new { Message = "Failed to delete user" });
        }

        [HttpPost("{userId}/favorites/{sportId}")]
        public async Task<IActionResult> AddSportToFavorites(int userId, int sportId)
        {
            var result = await _userService.AddSportToFavoritesAsync(userId, sportId);
            if (result)
                return Ok(new { Message = "Sport added to favorites" });

            return BadRequest(new { Message = "Failed to add sport to favorites" });
        }

        [HttpDelete("{userId}/favorites/{sportId}")]
        public async Task<IActionResult> RemoveSportFromFavorites(int userId, int sportId)
        {
            var result = await _userService.RemoveSportFromFavoritesAsync(userId, sportId);
            if (result)
                return Ok(new { Message = "Sport removed from favorites" });

            return BadRequest(new { Message = "Failed to remove sport from favorites" });
        }

        [HttpGet("{userId}/events/created")]
        public async Task<IActionResult> GetUserCreatedEvents(int userId)
        {
            var events = await _userService.GetUserCreatedEventsAsync(userId);
            return Ok(events);
        }

        [HttpGet("{userId}/events/participating")]
        public async Task<IActionResult> GetUserParticipatingEvents(int userId)
        {
            var events = await _userService.GetUserParticipatingEventsAsync(userId);
            return Ok(events);
        }
    }
} 