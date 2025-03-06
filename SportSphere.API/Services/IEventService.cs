using SportSphere.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public interface IEventService
    {
        Task<EventDto> GetEventByIdAsync(int id);
        Task<List<EventDto>> GetAllEventsAsync();
        Task<List<EventDto>> SearchEventsAsync(EventSearchDto searchDto);
        Task<List<EventDto>> GetNearbyEventsAsync(double latitude, double longitude, double radiusInKm);
        Task<int> CreateEventAsync(int creatorId, EventCreateDto eventDto);
        Task<bool> UpdateEventAsync(int id, EventUpdateDto eventDto);
        Task<bool> DeleteEventAsync(int id);
        Task<bool> JoinEventAsync(int eventId, int userId);
        Task<bool> LeaveEventAsync(int eventId, int userId);
        Task<bool> CancelEventAsync(int eventId);
        Task<List<UserDto>> GetEventParticipantsAsync(int eventId);
    }
} 