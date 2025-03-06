using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SportSphere.API.Data;
using SportSphere.Shared.DTOs;
using SportSphere.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportSphere.API.Services
{
    public class SportService : ISportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SportService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SportDto> GetSportByIdAsync(int id)
        {
            var sport = await _context.Sports.FindAsync(id);
            return _mapper.Map<SportDto>(sport);
        }

        public async Task<List<SportDto>> GetAllSportsAsync()
        {
            var sports = await _context.Sports.ToListAsync();
            return _mapper.Map<List<SportDto>>(sports);
        }

        public async Task<List<SportDto>> SearchSportsAsync(string searchTerm)
        {
            var sports = await _context.Sports
                .Where(s => s.Name.Contains(searchTerm) || s.Description.Contains(searchTerm))
                .ToListAsync();

            return _mapper.Map<List<SportDto>>(sports);
        }

        public async Task<int> CreateSportAsync(SportCreateDto sportDto)
        {
            var sport = _mapper.Map<SportModel>(sportDto);
            _context.Sports.Add(sport);
            await _context.SaveChangesAsync();
            return sport.Id;
        }

        public async Task<bool> UpdateSportAsync(int id, SportUpdateDto sportDto)
        {
            var sport = await _context.Sports.FindAsync(id);

            if (sport == null)
            {
                return false;
            }

            sport.Name = sportDto.Name;
            sport.Description = sportDto.Description;
            sport.IconUrl = sportDto.IconUrl;
            sport.IsPopular = sportDto.IsPopular;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSportAsync(int id)
        {
            var sport = await _context.Sports.FindAsync(id);

            if (sport == null)
            {
                return false;
            }

            _context.Sports.Remove(sport);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserDto>> GetSportPractitionersAsync(int sportId)
        {
            var users = await _context.Users
                .Include(u => u.FavoriteSports)
                .Include(u => u.DefaultLocation)
                .Where(u => u.FavoriteSports.Any(s => s.Id == sportId))
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<VenueDto>> GetSportVenuesAsync(int sportId)
        {
            var venues = await _context.Venues
                .Include(v => v.Location)
                .Include(v => v.OfferedSports)
                .Where(v => v.OfferedSports.Any(s => s.Id == sportId))
                .ToListAsync();

            return _mapper.Map<List<VenueDto>>(venues);
        }

        public async Task<List<EventDto>> GetSportEventsAsync(int sportId)
        {
            var events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .Where(e => e.SportId == sportId)
                .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
        }
    }
} 