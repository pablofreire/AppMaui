using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSphere.Shared.Models
{
    public class VenueModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<SportModel> OfferedSports { get; set; } = new List<SportModel>();
        
        [NotMapped]
        public List<SportModel> SupportedSports 
        { 
            get => OfferedSports; 
            set => OfferedSports = value; 
        }
        
        public List<EventModel> HostedEvents { get; set; } = new List<EventModel>();
        public LocationModel Location { get; set; } = null!;
        public int LocationId { get; set; }
        public string OpeningHours { get; set; } = string.Empty;
        public bool HasParking { get; set; }
        public bool HasShowers { get; set; }
        public bool HasLockers { get; set; }
        public decimal PricePerHour { get; set; }
    }
} 