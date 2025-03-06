using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSphere.Shared.Models
{
    public class SportModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public bool RequiresEquipment { get; set; }
        public bool IsTeamSport { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool IsPopular { get; set; }
        
        [NotMapped]
        public List<UserModel> Practitioners { get; set; } = new List<UserModel>();
        
        [NotMapped]
        public List<VenueModel> Venues { get; set; } = new List<VenueModel>();
        
        [NotMapped]
        public List<EventModel> Events { get; set; } = new List<EventModel>();
    }
} 