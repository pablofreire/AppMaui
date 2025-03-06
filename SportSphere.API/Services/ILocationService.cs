using SportSphere.Shared.DTOs;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public interface ILocationService
    {
        Task<LocationDto> GetLocationByIdAsync(int id);
        Task<int> CreateLocationAsync(LocationCreateDto locationDto);
        Task<bool> UpdateLocationAsync(int id, LocationUpdateDto locationDto);
        Task<bool> DeleteLocationAsync(int id);
        Task<double> CalculateDistanceAsync(double lat1, double lon1, double lat2, double lon2);
    }
} 