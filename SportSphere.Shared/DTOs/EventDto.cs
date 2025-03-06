using System;
using System.Collections.Generic;
using SportSphere.Shared.Models;

namespace SportSphere.Shared.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxParticipants { get; set; }
        public bool IsPublic { get; set; }
        public EventStatus Status { get; set; }
        public SportDto Sport { get; set; } = new SportDto();
        public UserDto Creator { get; set; } = new UserDto();
        public VenueDto Venue { get; set; } = new VenueDto();
        public LocationDto Location { get; set; } = new LocationDto();
        public List<UserDto> Participants { get; set; } = new List<UserDto>();
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool RequiresEquipment { get; set; }
        public string EquipmentDetails { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public int CurrentParticipants => Participants?.Count ?? 0;
    }

    public class EventCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxParticipants { get; set; }
        public bool IsPublic { get; set; }
        public int SportId { get; set; }
        public int? VenueId { get; set; }
        public LocationDto Location { get; set; } = new LocationDto();
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool RequiresEquipment { get; set; }
        public string EquipmentDetails { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
    }

    public class EventUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxParticipants { get; set; }
        public bool IsPublic { get; set; }
        public EventStatus Status { get; set; }
        public int SportId { get; set; }
        public int? VenueId { get; set; }
        public LocationDto Location { get; set; } = new LocationDto();
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool RequiresEquipment { get; set; }
        public string EquipmentDetails { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
    }

    public class EventSearchDto
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int? SportId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusInKm { get; set; }
        public bool? IsPublic { get; set; }
        public string SkillLevel { get; set; } = string.Empty;
        public decimal? MaxPrice { get; set; }
        public bool? HasAvailableSpots { get; set; }
    }
} 