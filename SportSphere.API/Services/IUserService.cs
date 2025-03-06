using SportSphere.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<List<UserDto>> SearchUsersAsync(string searchTerm);
        Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> AddSportToFavoritesAsync(int userId, int sportId);
        Task<bool> RemoveSportFromFavoritesAsync(int userId, int sportId);
        Task<List<EventDto>> GetUserCreatedEventsAsync(int userId);
        Task<List<EventDto>> GetUserParticipatingEventsAsync(int userId);
    }
} 