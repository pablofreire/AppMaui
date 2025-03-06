namespace SportSphere.Shared.DTOs
{
    public class SportDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public bool IsTeamSport { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool RequiresEquipment { get; set; }
        public bool IsPopular { get; set; }
    }

    public class SportCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public bool IsTeamSport { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool RequiresEquipment { get; set; }
        public bool IsPopular { get; set; }
    }

    public class SportUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public bool IsTeamSport { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool RequiresEquipment { get; set; }
        public bool IsPopular { get; set; }
    }
} 