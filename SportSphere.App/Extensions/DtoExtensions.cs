using System.Collections.Generic;
using System.Linq;
using SportSphere.Shared.DTOs;
using SportSphere.Shared.Models;

namespace SportSphere.App.Extensions
{
    public static class DtoExtensions
    {
        public static EventModel ToModel(this EventDto dto)
        {
            return new EventModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                MaxParticipants = dto.MaxParticipants,
                IsPublic = dto.IsPublic,
                Status = dto.Status,
                Sport = dto.Sport?.ToModel(),
                SportId = dto.Sport?.Id ?? 0,
                Creator = dto.Creator?.ToModel(),
                CreatorId = dto.Creator?.Id ?? 0,
                Venue = dto.Venue?.ToModel(),
                VenueId = dto.Venue?.Id,
                Location = dto.Location?.ToModel(),
                Participants = dto.Participants?.Select(p => p.ToModel()).ToList() ?? new List<UserModel>(),
                Price = dto.Price,
                RequiresEquipment = dto.RequiresEquipment,
                EquipmentDetails = dto.EquipmentDetails,
                SkillLevel = dto.SkillLevel
            };
        }

        public static SportModel ToModel(this SportDto dto)
        {
            if (dto == null) return null;
            
            return new SportModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IconUrl = dto.IconUrl,
                RequiresEquipment = dto.RequiresEquipment,
                IsTeamSport = dto.IsTeamSport,
                MinPlayers = dto.MinPlayers,
                MaxPlayers = dto.MaxPlayers,
                IsPopular = dto.IsPopular
            };
        }

        public static VenueModel ToModel(this VenueDto dto)
        {
            if (dto == null) return null;
            
            return new VenueModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                Location = dto.Location?.ToModel(),
                PhotoUrl = dto.PhotoUrl,
                ContactPhone = dto.ContactPhone,
                ContactEmail = dto.ContactEmail,
                WebsiteUrl = dto.WebsiteUrl,
                OpeningHours = dto.OpeningHours,
                HasParking = dto.HasParking,
                HasShowers = dto.HasShowers,
                HasLockers = dto.HasLockers,
                PricePerHour = dto.PricePerHour
            };
        }

        public static UserModel ToModel(this UserDto dto)
        {
            if (dto == null) return null;
            
            return new UserModel
            {
                Id = dto.Id,
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ProfilePictureUrl = dto.ProfilePictureUrl,
                Bio = dto.Bio,
                DateOfBirth = dto.DateOfBirth,
                RegistrationDate = dto.RegistrationDate,
                LastLoginDate = dto.LastLoginDate,
                Role = dto.Role,
                IsActive = dto.IsActive
            };
        }

        public static SportSphere.Shared.Models.LocationModel ToModel(this LocationDto dto)
        {
            if (dto == null) return null;
            
            return new SportSphere.Shared.Models.LocationModel
            {
                Id = dto.Id,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address
            };
        }
    }
} 