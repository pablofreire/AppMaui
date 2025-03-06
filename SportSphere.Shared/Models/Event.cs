using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSphere.Shared.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxParticipants { get; set; }
        public bool IsPublic { get; set; }
        
        [Column("Status")]
        public EventStatus Status { get; set; }
        
        public SportModel Sport { get; set; } = null!;
        public int SportId { get; set; }
        public UserModel Creator { get; set; } = null!;
        public int CreatorId { get; set; }
        public VenueModel? Venue { get; set; }
        public int? VenueId { get; set; }
        public LocationModel Location { get; set; } = null!;
        public int LocationId { get; set; }
        
        [NotMapped]
        public List<UserModel> Participants { get; set; } = new List<UserModel>();
        
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool RequiresEquipment { get; set; }
        public string EquipmentDetails { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    public enum EventStatus
    {
        Planned = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
} 