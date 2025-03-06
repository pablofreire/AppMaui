using SportSphere.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public interface IVenueService
    {
        Task<VenueDto> GetVenueByIdAsync(int id);
        Task<List<VenueDto>> GetAllVenuesAsync();
        Task<List<VenueDto>> SearchVenuesAsync(string searchTerm);
        Task<List<VenueDto>> GetVenuesByLocationAsync(double latitude, double longitude, double radiusInKm);
        Task<int> CreateVenueAsync(VenueCreateDto venueDto);
        Task<bool> UpdateVenueAsync(int id, VenueUpdateDto venueDto);
        Task<bool> DeleteVenueAsync(int id);
        Task<List<SportDto>> GetVenueSportsAsync(int venueId);
        Task<List<EventDto>> GetVenueEventsAsync(int venueId);
    }
} 