using SportSphere.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public interface ISportService
    {
        Task<SportDto> GetSportByIdAsync(int id);
        Task<List<SportDto>> GetAllSportsAsync();
        Task<List<SportDto>> SearchSportsAsync(string searchTerm);
        Task<int> CreateSportAsync(SportCreateDto sportDto);
        Task<bool> UpdateSportAsync(int id, SportUpdateDto sportDto);
        Task<bool> DeleteSportAsync(int id);
        Task<List<UserDto>> GetSportPractitionersAsync(int sportId);
        Task<List<VenueDto>> GetSportVenuesAsync(int sportId);
        Task<List<EventDto>> GetSportEventsAsync(int sportId);
    }
} 