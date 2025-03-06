using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportSphere.Shared.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public int DefaultLocationId { get; set; }
        public string Role { get; set; } = string.Empty;
        
        public LocationModel DefaultLocation { get; set; } = null!;
        
        [NotMapped]
        public List<SportModel> FavoriteSports { get; set; } = new List<SportModel>();
        
        [NotMapped]
        public List<EventModel> CreatedEvents { get; set; } = new List<EventModel>();
        
        [NotMapped]
        public List<EventModel> ParticipatingEvents { get; set; } = new List<EventModel>();
    }
} 