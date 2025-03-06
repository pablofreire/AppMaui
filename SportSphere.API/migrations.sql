IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Locations] (
    [Id] int NOT NULL IDENTITY,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Sports] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [IconUrl] nvarchar(max) NOT NULL,
    [RequiresEquipment] bit NOT NULL,
    [IsTeamSport] bit NOT NULL,
    [MinPlayers] int NOT NULL,
    [MaxPlayers] int NOT NULL,
    [IsPopular] bit NOT NULL,
    CONSTRAINT [PK_Sports] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [ProfilePictureUrl] nvarchar(max) NOT NULL,
    [Bio] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [LastLogin] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [DefaultLocationId] int NOT NULL,
    [RegistrationDate] datetime2 NOT NULL,
    [LastLoginDate] datetime2 NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Locations_DefaultLocationId] FOREIGN KEY ([DefaultLocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Venues] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [ContactPhone] nvarchar(max) NOT NULL,
    [ContactEmail] nvarchar(max) NOT NULL,
    [WebsiteUrl] nvarchar(max) NOT NULL,
    [PhotoUrl] nvarchar(max) NOT NULL,
    [IsVerified] bit NOT NULL,
    [Rating] decimal(18,2) NOT NULL,
    [ReviewCount] int NOT NULL,
    [LocationId] int NOT NULL,
    [OpeningHours] nvarchar(max) NOT NULL,
    [HasParking] bit NOT NULL,
    [HasShowers] bit NOT NULL,
    [HasLockers] bit NOT NULL,
    [PricePerHour] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Venues_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [UserFavoriteSports] (
    [FavoriteSportsId] int NOT NULL,
    [PractitionersId] int NOT NULL,
    CONSTRAINT [PK_UserFavoriteSports] PRIMARY KEY ([FavoriteSportsId], [PractitionersId]),
    CONSTRAINT [FK_UserFavoriteSports_Sports_FavoriteSportsId] FOREIGN KEY ([FavoriteSportsId]) REFERENCES [Sports] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserFavoriteSports_Users_PractitionersId] FOREIGN KEY ([PractitionersId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Events] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [StartTime] datetime2 NOT NULL,
    [EndTime] datetime2 NOT NULL,
    [MaxParticipants] int NOT NULL,
    [IsPublic] bit NOT NULL,
    [Status] int NOT NULL,
    [SportId] int NOT NULL,
    [CreatorId] int NOT NULL,
    [VenueId] int NULL,
    [LocationId] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [RequiresEquipment] bit NOT NULL,
    [EquipmentDetails] nvarchar(max) NOT NULL,
    [SkillLevel] nvarchar(max) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [LastUpdateDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Events_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Events_Sports_SportId] FOREIGN KEY ([SportId]) REFERENCES [Sports] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Events_Users_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [VenueSports] (
    [OfferedSportsId] int NOT NULL,
    [VenuesId] int NOT NULL,
    CONSTRAINT [PK_VenueSports] PRIMARY KEY ([OfferedSportsId], [VenuesId]),
    CONSTRAINT [FK_VenueSports_Sports_OfferedSportsId] FOREIGN KEY ([OfferedSportsId]) REFERENCES [Sports] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VenueSports_Venues_VenuesId] FOREIGN KEY ([VenuesId]) REFERENCES [Venues] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [EventParticipants] (
    [ParticipantsId] int NOT NULL,
    [ParticipatingEventsId] int NOT NULL,
    CONSTRAINT [PK_EventParticipants] PRIMARY KEY ([ParticipantsId], [ParticipatingEventsId]),
    CONSTRAINT [FK_EventParticipants_Events_ParticipatingEventsId] FOREIGN KEY ([ParticipatingEventsId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EventParticipants_Users_ParticipantsId] FOREIGN KEY ([ParticipantsId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_EventParticipants_ParticipatingEventsId] ON [EventParticipants] ([ParticipatingEventsId]);
GO

CREATE INDEX [IX_Events_CreatorId] ON [Events] ([CreatorId]);
GO

CREATE UNIQUE INDEX [IX_Events_LocationId] ON [Events] ([LocationId]);
GO

CREATE INDEX [IX_Events_SportId] ON [Events] ([SportId]);
GO

CREATE INDEX [IX_Events_VenueId] ON [Events] ([VenueId]);
GO

CREATE INDEX [IX_UserFavoriteSports_PractitionersId] ON [UserFavoriteSports] ([PractitionersId]);
GO

CREATE UNIQUE INDEX [IX_Users_DefaultLocationId] ON [Users] ([DefaultLocationId]);
GO

CREATE UNIQUE INDEX [IX_Venues_LocationId] ON [Venues] ([LocationId]);
GO

CREATE INDEX [IX_VenueSports_VenuesId] ON [VenueSports] ([VenuesId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250305163854_InitialCreate', N'8.0.0');
GO

COMMIT;
GO

