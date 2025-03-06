using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportSphere.API.Data;
using SportSphere.Shared.DTOs;
using SportSphere.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EventService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventDto> GetEventByIdAsync(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<EventDto>(@event);
        }

        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            var events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .Where(e => e.StartTime > DateTime.UtcNow)
                .OrderBy(e => e.StartTime)
                .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> SearchEventsAsync(EventSearchDto searchDto)
        {
            var query = _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .AsQueryable();

            // Filtrar por termo de pesquisa
            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                query = query.Where(e => 
                    e.Title.Contains(searchDto.SearchTerm) || 
                    e.Description.Contains(searchDto.SearchTerm));
            }

            // Filtrar por esporte
            if (searchDto.SportId.HasValue)
            {
                query = query.Where(e => e.SportId == searchDto.SportId.Value);
            }

            // Filtrar por data
            if (searchDto.StartDate.HasValue)
            {
                query = query.Where(e => e.StartTime.Date >= searchDto.StartDate.Value.Date);
            }

            if (searchDto.EndDate.HasValue)
            {
                query = query.Where(e => e.StartTime.Date <= searchDto.EndDate.Value.Date);
            }

            // Filtrar por nível de habilidade
            if (!string.IsNullOrEmpty(searchDto.SkillLevel))
            {
                query = query.Where(e => e.SkillLevel == searchDto.SkillLevel);
            }

            // Filtrar por vagas disponíveis
            if (searchDto.HasAvailableSpots.HasValue && searchDto.HasAvailableSpots.Value)
            {
                query = query.Where(e => e.MaxParticipants > e.Participants.Count);
            }

            // Ordenar por data
            query = query.OrderBy(e => e.StartTime);

            var events = await query.ToListAsync();
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetNearbyEventsAsync(double latitude, double longitude, double radiusInKm)
        {
            // Constante para converter de km para graus (aproximadamente)
            const double kmToDegrees = 0.01;
            double radiusInDegrees = radiusInKm * kmToDegrees;

            var events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .Where(e => 
                    e.Location.Latitude >= latitude - radiusInDegrees &&
                    e.Location.Latitude <= latitude + radiusInDegrees &&
                    e.Location.Longitude >= longitude - radiusInDegrees &&
                    e.Location.Longitude <= longitude + radiusInDegrees &&
                    e.StartTime > DateTime.UtcNow)
                .ToListAsync();

            // Filtrar mais precisamente usando a fórmula de Haversine
            var filteredEvents = events.Where(e => 
                CalculateDistance(latitude, longitude, e.Location.Latitude, e.Location.Longitude) <= radiusInKm)
                .OrderBy(e => e.StartTime)
                .ToList();

            return _mapper.Map<List<EventDto>>(filteredEvents);
        }

        public async Task<int> CreateEventAsync(int creatorId, EventCreateDto eventDto)
        {
            var user = await _context.Users.FindAsync(creatorId);
            if (user == null)
            {
                throw new ArgumentException("Usuário criador não encontrado.");
            }

            var sport = await _context.Sports.FindAsync(eventDto.SportId);
            if (sport == null)
            {
                throw new ArgumentException("Esporte não encontrado.");
            }

            var venue = eventDto.VenueId.HasValue ? await _context.Venues.FindAsync(eventDto.VenueId.Value) : null;

            var @event = new EventModel
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                StartTime = eventDto.StartTime,
                EndTime = eventDto.EndTime,
                CreationDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow,
                SportId = eventDto.SportId,
                Sport = sport,
                CreatorId = creatorId,
                Creator = user,
                VenueId = eventDto.VenueId,
                Venue = venue,
                Location = new LocationModel
                {
                    Latitude = eventDto.Location.Latitude,
                    Longitude = eventDto.Location.Longitude,
                    Address = eventDto.Location.Address,
                    City = eventDto.Location.City,
                    State = eventDto.Location.State,
                    Country = eventDto.Location.Country,
                    PostalCode = eventDto.Location.PostalCode
                },
                ImageUrl = eventDto.ImageUrl,
                MaxParticipants = eventDto.MaxParticipants,
                EquipmentDetails = eventDto.EquipmentDetails,
                SkillLevel = eventDto.SkillLevel,
                Status = EventStatus.Planned,
                Participants = new List<UserModel> { user } // O criador é automaticamente um participante
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return @event.Id;
        }

        public async Task<bool> UpdateEventAsync(int id, EventUpdateDto eventDto)
        {
            var @event = await _context.Events
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return false;
            }

            // Atualizar propriedades básicas
            @event.Title = eventDto.Title;
            @event.Description = eventDto.Description;
            @event.StartTime = eventDto.StartTime;
            @event.EndTime = eventDto.EndTime;
            @event.LastUpdateDate = DateTime.UtcNow;
            @event.SportId = eventDto.SportId;
            @event.VenueId = eventDto.VenueId;
            @event.ImageUrl = eventDto.ImageUrl;
            @event.MaxParticipants = eventDto.MaxParticipants;
            @event.EquipmentDetails = eventDto.EquipmentDetails;
            @event.SkillLevel = eventDto.SkillLevel;

            // Atualizar localização
            if (eventDto.Location != null)
            {
                if (@event.Location == null)
                {
                    @event.Location = new LocationModel();
                }

                @event.Location.Latitude = eventDto.Location.Latitude;
                @event.Location.Longitude = eventDto.Location.Longitude;
                @event.Location.Address = eventDto.Location.Address;
                @event.Location.City = eventDto.Location.City;
                @event.Location.State = eventDto.Location.State;
                @event.Location.Country = eventDto.Location.Country;
                @event.Location.PostalCode = eventDto.Location.PostalCode;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return false;
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> JoinEventAsync(int eventId, int userId)
        {
            var @event = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (@event == null)
            {
                return false;
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Verificar se o evento já está cheio
            if (@event.MaxParticipants > 0 && @event.Participants.Count >= @event.MaxParticipants)
            {
                return false;
            }

            // Verificar se o usuário já é um participante
            if (@event.Participants.Any(p => p.Id == userId))
            {
                return true; // Já é participante, consideramos sucesso
            }

            @event.Participants.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveEventAsync(int eventId, int userId)
        {
            var @event = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (@event == null)
            {
                return false;
            }

            var user = @event.Participants.FirstOrDefault(p => p.Id == userId);
            if (user == null)
            {
                return false; // Usuário não é participante
            }

            // Se o usuário é o criador do evento, não pode sair
            if (@event.CreatorId == userId)
            {
                return false;
            }

            @event.Participants.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelEventAsync(int eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);

            if (@event == null)
            {
                return false;
            }

            @event.Status = EventStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserDto>> GetEventParticipantsAsync(int eventId)
        {
            var @event = await _context.Events
                .Include(e => e.Participants)
                .ThenInclude(p => p.DefaultLocation)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (@event == null)
            {
                return new List<UserDto>();
            }

            return _mapper.Map<List<UserDto>>(@event.Participants);
        }

        // Método auxiliar para calcular a distância entre dois pontos usando a fórmula de Haversine
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadiusKm = 6371.0;
            
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadiusKm * c;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
} 