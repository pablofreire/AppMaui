using SportSphere.Shared.DTOs;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, string Message)> LoginAsync(UserLoginDto loginDto);
        Task<(bool Success, string Message)> RegisterAsync(UserRegistrationDto registrationDto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
} 