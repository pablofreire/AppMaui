using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportSphere.API.Data;
using SportSphere.Shared.DTOs;
using SportSphere.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LocationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LocationDto> GetLocationByIdAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            return _mapper.Map<LocationDto>(location);
        }

        public async Task<int> CreateLocationAsync(LocationCreateDto locationDto)
        {
            var location = _mapper.Map<LocationModel>(locationDto);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location.Id;
        }

        public async Task<bool> UpdateLocationAsync(int id, LocationUpdateDto locationDto)
        {
            var location = await _context.Locations.FindAsync(id);
            
            if (location == null)
            {
                return false;
            }

            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;
            location.Address = locationDto.Address;
            location.City = locationDto.City;
            location.State = locationDto.State;
            location.Country = locationDto.Country;
            location.PostalCode = locationDto.PostalCode;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            
            if (location == null)
            {
                return false;
            }

            // Verificar se a localização está sendo usada por usuários ou eventos
            var isUsedByUser = await _context.Users.AnyAsync(u => u.DefaultLocationId == id);
            var isUsedByEvent = await _context.Events.AnyAsync(e => e.LocationId == id);
            var isUsedByVenue = await _context.Venues.AnyAsync(v => v.LocationId == id);

            if (isUsedByUser || isUsedByEvent || isUsedByVenue)
            {
                return false; // Não podemos excluir uma localização que está em uso
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<double> CalculateDistanceAsync(double lat1, double lon1, double lat2, double lon2)
        {
            // Implementação da fórmula de Haversine para calcular a distância entre dois pontos geográficos
            const double earthRadiusKm = 6371.0;
            
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadiusKm * c;
            
            return Task.FromResult(distance);
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
} 