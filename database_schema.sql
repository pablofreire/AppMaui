-- Criação do banco de dados
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SportSphere')
BEGIN
    CREATE DATABASE SportSphere;
END
GO

USE SportSphere;
GO

-- Tabela de Localização
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Location')
BEGIN
    CREATE TABLE Location (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Latitude FLOAT NOT NULL,
        Longitude FLOAT NOT NULL,
        Address NVARCHAR(255) NOT NULL,
        City NVARCHAR(100) NOT NULL,
        State NVARCHAR(100) NOT NULL,
        Country NVARCHAR(100) NOT NULL,
        PostalCode NVARCHAR(20) NOT NULL
    );
END
GO

-- Tabela de Esportes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sport')
BEGIN
    CREATE TABLE Sport (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        IconUrl NVARCHAR(255) NOT NULL,
        RequiresEquipment BIT NOT NULL DEFAULT 0,
        IsTeamSport BIT NOT NULL DEFAULT 0,
        MinPlayers INT NOT NULL DEFAULT 1,
        MaxPlayers INT NOT NULL DEFAULT 1,
        IsPopular BIT NOT NULL DEFAULT 0
    );
END
GO

-- Tabela de Usuários
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
BEGIN
    CREATE TABLE [User] (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        ProfilePictureUrl NVARCHAR(255) NOT NULL DEFAULT '',
        Bio NVARCHAR(MAX) NOT NULL DEFAULT '',
        DateOfBirth DATETIME NOT NULL,
        RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
        LastLoginDate DATETIME NOT NULL DEFAULT GETDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        DefaultLocationId INT NOT NULL,
        Role NVARCHAR(50) NOT NULL DEFAULT 'User',
        FOREIGN KEY (DefaultLocationId) REFERENCES Location(Id)
    );
END
GO

-- Tabela de Locais (Venues)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Venue')
BEGIN
    CREATE TABLE Venue (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        Address NVARCHAR(255) NOT NULL,
        ContactPhone NVARCHAR(20) NOT NULL,
        ContactEmail NVARCHAR(100) NOT NULL,
        WebsiteUrl NVARCHAR(255) NOT NULL DEFAULT '',
        PhotoUrl NVARCHAR(255) NOT NULL DEFAULT '',
        IsVerified BIT NOT NULL DEFAULT 0,
        Rating DECIMAL(3,2) NOT NULL DEFAULT 0.0,
        ReviewCount INT NOT NULL DEFAULT 0,
        LocationId INT NOT NULL,
        OpeningHours NVARCHAR(255) NOT NULL DEFAULT '',
        HasParking BIT NOT NULL DEFAULT 0,
        HasShowers BIT NOT NULL DEFAULT 0,
        HasLockers BIT NOT NULL DEFAULT 0,
        PricePerHour DECIMAL(10,2) NOT NULL DEFAULT 0.0,
        FOREIGN KEY (LocationId) REFERENCES Location(Id)
    );
END
GO

-- Tabela de Eventos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Event')
BEGIN
    CREATE TABLE Event (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        StartTime DATETIME NOT NULL,
        EndTime DATETIME NOT NULL,
        MaxParticipants INT NOT NULL,
        IsPublic BIT NOT NULL DEFAULT 1,
        Status INT NOT NULL DEFAULT 0, -- 0: Planned, 1: InProgress, 2: Completed, 3: Cancelled
        SportId INT NOT NULL,
        CreatorId INT NOT NULL,
        VenueId INT NULL,
        LocationId INT NOT NULL,
        ImageUrl NVARCHAR(255) NOT NULL DEFAULT '',
        Price DECIMAL(10,2) NOT NULL DEFAULT 0.0,
        RequiresEquipment BIT NOT NULL DEFAULT 0,
        EquipmentDetails NVARCHAR(MAX) NOT NULL DEFAULT '',
        SkillLevel NVARCHAR(50) NOT NULL DEFAULT '',
        CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
        LastUpdateDate DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (SportId) REFERENCES Sport(Id),
        FOREIGN KEY (CreatorId) REFERENCES [User](Id),
        FOREIGN KEY (VenueId) REFERENCES Venue(Id),
        FOREIGN KEY (LocationId) REFERENCES Location(Id)
    );
END
GO

-- Tabela de relacionamento entre Usuários e Esportes favoritos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserFavoriteSport')
BEGIN
    CREATE TABLE UserFavoriteSport (
        UserId INT NOT NULL,
        SportId INT NOT NULL,
        PRIMARY KEY (UserId, SportId),
        FOREIGN KEY (UserId) REFERENCES [User](Id),
        FOREIGN KEY (SportId) REFERENCES Sport(Id)
    );
END
GO

-- Tabela de relacionamento entre Usuários e Eventos (participantes)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EventParticipant')
BEGIN
    CREATE TABLE EventParticipant (
        EventId INT NOT NULL,
        UserId INT NOT NULL,
        PRIMARY KEY (EventId, UserId),
        FOREIGN KEY (EventId) REFERENCES Event(Id),
        FOREIGN KEY (UserId) REFERENCES [User](Id)
    );
END
GO

-- Tabela de relacionamento entre Locais (Venues) e Esportes oferecidos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VenueSport')
BEGIN
    CREATE TABLE VenueSport (
        VenueId INT NOT NULL,
        SportId INT NOT NULL,
        PRIMARY KEY (VenueId, SportId),
        FOREIGN KEY (VenueId) REFERENCES Venue(Id),
        FOREIGN KEY (SportId) REFERENCES Sport(Id)
    );
END
GO 