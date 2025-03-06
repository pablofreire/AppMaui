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
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.DefaultLocation)
                .Include(u => u.FavoriteSports)
                .FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.DefaultLocation)
                .Include(u => u.FavoriteSports)
                .FirstOrDefaultAsync(u => u.Email == email);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.DefaultLocation)
                .Include(u => u.FavoriteSports)
                .FirstOrDefaultAsync(u => u.Username == username);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.DefaultLocation)
                .Include(u => u.FavoriteSports)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _context.Users
                .Include(u => u.DefaultLocation)
                .Include(u => u.FavoriteSports)
                .Where(u => u.Username.Contains(searchTerm) || 
                           u.FirstName.Contains(searchTerm) || 
                           u.LastName.Contains(searchTerm) || 
                           u.Email.Contains(searchTerm))
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            var user = await _context.Users
                .Include(u => u.DefaultLocation)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            // Atualizar propriedades básicas
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Bio = userDto.Bio;
            user.ProfilePictureUrl = userDto.ProfilePictureUrl;

            // Atualizar localização padrão
            if (userDto.DefaultLocation != null)
            {
                if (user.DefaultLocation == null)
                {
                    user.DefaultLocation = new LocationModel();
                }

                user.DefaultLocation.Latitude = userDto.DefaultLocation.Latitude;
                user.DefaultLocation.Longitude = userDto.DefaultLocation.Longitude;
                user.DefaultLocation.Address = userDto.DefaultLocation.Address;
                user.DefaultLocation.City = userDto.DefaultLocation.City;
                user.DefaultLocation.State = userDto.DefaultLocation.State;
                user.DefaultLocation.Country = userDto.DefaultLocation.Country;
                user.DefaultLocation.PostalCode = userDto.DefaultLocation.PostalCode;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSportToFavoritesAsync(int userId, int sportId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteSports)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var sport = await _context.Sports.FindAsync(sportId);

            if (sport == null)
            {
                return false;
            }

            if (user.FavoriteSports == null)
            {
                user.FavoriteSports = new List<SportModel>();
            }

            if (!user.FavoriteSports.Any(s => s.Id == sportId))
            {
                user.FavoriteSports.Add(sport);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveSportFromFavoritesAsync(int userId, int sportId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteSports)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FavoriteSports == null)
            {
                return false;
            }

            var sport = user.FavoriteSports.FirstOrDefault(s => s.Id == sportId);

            if (sport == null)
            {
                return false;
            }

            user.FavoriteSports.Remove(sport);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EventDto>> GetUserCreatedEventsAsync(int userId)
        {
            var events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .Where(e => e.CreatorId == userId)
                .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetUserParticipatingEventsAsync(int userId)
        {
            var events = await _context.Events
                .Include(e => e.Creator)
                .Include(e => e.Sport)
                .Include(e => e.Venue)
                .Include(e => e.Location)
                .Include(e => e.Participants)
                .Where(e => e.Participants.Any(p => p.Id == userId))
                .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
        }
    }
} 