using System.Collections.Generic;

namespace SportSphere.Shared.DTOs
{
    public class VenueDto
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
        public List<SportDto> OfferedSports { get; set; } = new List<SportDto>();
        public LocationDto Location { get; set; } = new LocationDto();
        public string OpeningHours { get; set; } = string.Empty;
        public bool HasParking { get; set; }
        public bool HasShowers { get; set; }
        public bool HasLockers { get; set; }
        public decimal PricePerHour { get; set; }
    }

    public class VenueCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public List<int> OfferedSportIds { get; set; } = new List<int>();
        public LocationDto Location { get; set; } = new LocationDto();
        public string OpeningHours { get; set; } = string.Empty;
        public bool HasParking { get; set; }
        public bool HasShowers { get; set; }
        public bool HasLockers { get; set; }
        public decimal PricePerHour { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }

    public class VenueUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public List<int> OfferedSportIds { get; set; } = new List<int>();
        public LocationDto Location { get; set; } = new LocationDto();
        public string OpeningHours { get; set; } = string.Empty;
        public bool HasParking { get; set; }
        public bool HasShowers { get; set; }
        public bool HasLockers { get; set; }
        public decimal PricePerHour { get; set; }
        public bool IsVerified { get; set; }
    }
} 