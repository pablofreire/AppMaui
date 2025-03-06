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
    public class VenueService : IVenueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VenueService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VenueDto> GetVenueByIdAsync(int id)
        {
            var venue = await _context.Venues
                .Include(v => v.Location)
                .Include(v => v.OfferedSports)
                .FirstOrDefaultAsync(v => v.Id == id);

            return _mapper.Map<VenueDto>(venue);
        }

        public async Task<List<VenueDto>> GetAllVenuesAsync()
        {
            var venues = await _context.Venues
                .Include(v => v.Location)
                .Include(v => v.OfferedSports)
                .ToListAsync();

            return _mapper.Map<List<VenueDto>>(venues);
        }

        public async Task<List<VenueDto>> SearchVenuesAsync(string searchTerm)
        {
            var venues = await _context.Venues
                .Include(v => v.Location)
                .Include(v => v.OfferedSports)
                .Where(v => v.Name.Contains(searchTerm) || 
                           v.Description.Contains(searchTerm) || 
                           v.Address.Contains(searchTerm) ||
                           v.Location.City.Contains(searchTerm) ||
                           v.Location.State.Contains(searchTerm))
                .ToListAsync();

            return _mapper.Map<List<VenueDto>>(venues);
        }

        public async Task<List<VenueDto>> GetVenuesByLocationAsync(double latitude, double longitude, double radiusInKm)
        {
            // Constante para converter de km para graus (aproximadamente)
            const double kmToDegrees = 0.01;
            double radiusInDegrees = radiusInKm * kmToDegrees;

            var venues = await _context.Venues
                .Include(v => v.Location)
                .Include(v => v.OfferedSports)
                .Where(v => 
                    v.Location.Latitude >= latitude - radiusInDegrees &&
                    v.Location.Latitude <= latitude + radiusInDegrees &&
                    v.Location.Longitude >= longitude - radiusInDegrees &&
                    v.Location.Longitude <= longitude + radiusInDegrees)
                .ToListAsync();

            // Filtrar mais precisamente usando a fórmula de Haversine
            var filteredVenues = venues.Where(v => 
                CalculateDistance(latitude, longitude, v.Location.Latitude, v.Location.Longitude) <= radiusInKm)
                .ToList();

            return _mapper.Map<List<VenueDto>>(filteredVenues);
        }

        public async Task<int> CreateVenueAsync(VenueCreateDto venueDto)
        {
            var venue = _mapper.Map<VenueModel>(venueDto);
            
            // Criar localização se não existir
            if (venue.Location == null)
            {
                venue.Location = new LocationModel
                {
                    Latitude = venueDto.Latitude,
                    Longitude = venueDto.Longitude,
                    Address = venueDto.Address,
                    City = venueDto.City ?? "Cidade não especificada",
                    State = venueDto.State ?? "Estado não especificado",
                    Country = venueDto.Country ?? "País não especificado",
                    PostalCode = venueDto.PostalCode ?? "00000-000"
                };
            }

            // Adicionar esportes oferecidos
            if (venueDto.OfferedSportIds != null && venueDto.OfferedSportIds.Any())
            {
                venue.OfferedSports = new List<SportModel>();
                foreach (var sportId in venueDto.OfferedSportIds)
                {
                    var sport = await _context.Sports.FindAsync(sportId);
                    if (sport != null)
                    {
                        venue.OfferedSports.Add(sport);
                    }
                }
            }

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return venue.Id;
        }

        public async Task<bool> UpdateVenueAsync(int id, VenueUpdateDto venueDto)
        {
            var venue = await _context.Venues
                .Include(v => v.Location)
                .Include(v => v.OfferedSports)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venue == null)
            {
                return false;
            }

            // Atualizar propriedades básicas
            venue.Name = venueDto.Name;
            venue.Description = venueDto.Description;
            venue.Address = venueDto.Address;
            venue.ContactPhone = venueDto.ContactPhone;
            venue.ContactEmail = venueDto.ContactEmail;
            venue.WebsiteUrl = venueDto.WebsiteUrl;
            venue.PhotoUrl = venueDto.PhotoUrl;
            venue.IsVerified = venueDto.IsVerified;
            venue.OpeningHours = venueDto.OpeningHours;

            // Atualizar localização
            if (venue.Location == null)
            {
                venue.Location = new LocationModel();
            }

            venue.Location.Latitude = venueDto.Location.Latitude;
            venue.Location.Longitude = venueDto.Location.Longitude;
            venue.Location.Address = venueDto.Location.Address;
            venue.Location.City = venueDto.Location.City;
            venue.Location.State = venueDto.Location.State;
            venue.Location.Country = venueDto.Location.Country;
            venue.Location.PostalCode = venueDto.Location.PostalCode;

            // Atualizar esportes oferecidos
            if (venueDto.OfferedSportIds != null)
            {
                // Limpar esportes existentes
                venue.OfferedSports.Clear();

                // Adicionar novos esportes
                foreach (var sportId in venueDto.OfferedSportIds)
                {
                    var sport = await _context.Sports.FindAsync(sportId);
                    if (sport != null)
                    {
                        venue.OfferedSports.Add(sport);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVenueAsync(int id)
        {
            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
            {
                return false;
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SportDto>> GetVenueSportsAsync(int venueId)
        {
            var venue = await _context.Venues
                .Include(v => v.OfferedSports)
                .FirstOrDefaultAsync(v => v.Id == venueId);

            if (venue == null || venue.OfferedSports == null)
            {
                return new List<SportDto>();
            }

            return _mapper.Map<List<SportDto>>(venue.OfferedSports);
        }

        public async Task<List<EventDto>> GetVenueEventsAsync(int venueId)
        {
            var events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .Where(e => e.VenueId == venueId)
                .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
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