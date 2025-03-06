USE SportSphere;
GO

-- Inserir localizações
INSERT INTO Location (Latitude, Longitude, Address, City, State, Country, PostalCode)
VALUES 
    (-23.5505, -46.6333, 'Av. Paulista, 1578', 'São Paulo', 'SP', 'Brasil', '01310-200'),
    (-22.9068, -43.1729, 'Av. Atlântica, 1702', 'Rio de Janeiro', 'RJ', 'Brasil', '22021-001'),
    (-19.9167, -43.9345, 'Av. Afonso Pena, 1500', 'Belo Horizonte', 'MG', 'Brasil', '30130-921'),
    (-25.4284, -49.2733, 'Rua XV de Novembro, 300', 'Curitiba', 'PR', 'Brasil', '80020-310'),
    (-30.0346, -51.2177, 'Av. Ipiranga, 7200', 'Porto Alegre', 'RS', 'Brasil', '90610-000'),
    (-12.9714, -38.5014, 'Av. Oceânica, 2500', 'Salvador', 'BA', 'Brasil', '40170-010'),
    (-8.0476, -34.8770, 'Av. Boa Viagem, 1200', 'Recife', 'PE', 'Brasil', '51011-000'),
    (-3.7319, -38.5267, 'Av. Beira Mar, 3000', 'Fortaleza', 'CE', 'Brasil', '60165-121'),
    (-16.6799, -49.2550, 'Av. Goiás, 500', 'Goiânia', 'GO', 'Brasil', '74010-010'),
    (-15.7801, -47.9292, 'Eixo Monumental, SN', 'Brasília', 'DF', 'Brasil', '70070-350');
GO

-- Inserir esportes
INSERT INTO Sport (Name, Description, IconUrl, RequiresEquipment, IsTeamSport, MinPlayers, MaxPlayers, IsPopular)
VALUES 
    ('Futebol', 'O esporte mais popular do Brasil', 'https://example.com/icons/soccer.png', 1, 1, 2, 22, 1),
    ('Vôlei', 'Esporte coletivo jogado em quadra ou areia', 'https://example.com/icons/volleyball.png', 1, 1, 2, 12, 1),
    ('Basquete', 'Esporte coletivo jogado em quadra', 'https://example.com/icons/basketball.png', 1, 1, 2, 10, 1),
    ('Tênis', 'Esporte de raquete jogado em quadra', 'https://example.com/icons/tennis.png', 1, 0, 2, 4, 1),
    ('Natação', 'Esporte aquático individual ou em equipe', 'https://example.com/icons/swimming.png', 1, 0, 1, 8, 1),
    ('Corrida', 'Atividade física de resistência', 'https://example.com/icons/running.png', 0, 0, 1, 100, 1),
    ('Ciclismo', 'Esporte praticado com bicicleta', 'https://example.com/icons/cycling.png', 1, 0, 1, 50, 1),
    ('Yoga', 'Prática que combina posturas físicas, respiração e meditação', 'https://example.com/icons/yoga.png', 0, 0, 1, 30, 1),
    ('Crossfit', 'Treinamento de alta intensidade que combina diferentes exercícios', 'https://example.com/icons/crossfit.png', 1, 0, 1, 20, 1),
    ('Surf', 'Esporte aquático praticado nas ondas do mar', 'https://example.com/icons/surf.png', 1, 0, 1, 10, 1);
GO

-- Inserir usuários (senha hash fictício)
INSERT INTO [User] (Username, Email, PasswordHash, FirstName, LastName, ProfilePictureUrl, Bio, DateOfBirth, RegistrationDate, LastLoginDate, IsActive, DefaultLocationId, Role)
VALUES 
    ('joaosilva', 'joao.silva@email.com', 'hash123', 'João', 'Silva', 'https://example.com/profiles/joao.jpg', 'Apaixonado por esportes', '1990-05-15', GETDATE(), GETDATE(), 1, 1, 'Admin'),
    ('mariasantos', 'maria.santos@email.com', 'hash456', 'Maria', 'Santos', 'https://example.com/profiles/maria.jpg', 'Atleta amadora', '1992-08-20', GETDATE(), GETDATE(), 1, 2, 'User'),
    ('pedroalves', 'pedro.alves@email.com', 'hash789', 'Pedro', 'Alves', 'https://example.com/profiles/pedro.jpg', 'Praticante de esportes radicais', '1988-03-10', GETDATE(), GETDATE(), 1, 3, 'User'),
    ('analuiza', 'ana.luiza@email.com', 'hash101', 'Ana', 'Luiza', 'https://example.com/profiles/ana.jpg', 'Nadadora profissional', '1995-11-25', GETDATE(), GETDATE(), 1, 4, 'User'),
    ('carlosoliveira', 'carlos.oliveira@email.com', 'hash202', 'Carlos', 'Oliveira', 'https://example.com/profiles/carlos.jpg', 'Jogador de futebol amador', '1985-07-30', GETDATE(), GETDATE(), 1, 5, 'User'),
    ('julianacosta', 'juliana.costa@email.com', 'hash303', 'Juliana', 'Costa', 'https://example.com/profiles/juliana.jpg', 'Instrutora de yoga', '1993-02-18', GETDATE(), GETDATE(), 1, 6, 'User'),
    ('lucaspereira', 'lucas.pereira@email.com', 'hash404', 'Lucas', 'Pereira', 'https://example.com/profiles/lucas.jpg', 'Ciclista de fim de semana', '1991-09-05', GETDATE(), GETDATE(), 1, 7, 'User'),
    ('fernandamartins', 'fernanda.martins@email.com', 'hash505', 'Fernanda', 'Martins', 'https://example.com/profiles/fernanda.jpg', 'Corredora de maratonas', '1987-12-12', GETDATE(), GETDATE(), 1, 8, 'User'),
    ('rodrigogomes', 'rodrigo.gomes@email.com', 'hash606', 'Rodrigo', 'Gomes', 'https://example.com/profiles/rodrigo.jpg', 'Surfista nas horas vagas', '1994-04-22', GETDATE(), GETDATE(), 1, 9, 'User'),
    ('camilarocha', 'camila.rocha@email.com', 'hash707', 'Camila', 'Rocha', 'https://example.com/profiles/camila.jpg', 'Praticante de crossfit', '1989-06-08', GETDATE(), GETDATE(), 1, 10, 'User');
GO

-- Inserir locais (venues)
INSERT INTO Venue (Name, Description, Address, ContactPhone, ContactEmail, WebsiteUrl, PhotoUrl, IsVerified, Rating, ReviewCount, LocationId, OpeningHours, HasParking, HasShowers, HasLockers, PricePerHour)
VALUES 
    ('Estádio Municipal', 'Estádio de futebol com capacidade para 20.000 pessoas', 'Rua do Estádio, 100', '(11) 1234-5678', 'estadio@email.com', 'https://estadio.com.br', 'https://example.com/venues/estadio.jpg', 1, 4.5, 120, 1, 'Seg-Dom: 08:00-22:00', 1, 1, 1, 200.00),
    ('Arena Vôlei', 'Quadras de vôlei indoor e de areia', 'Av. das Quadras, 200', '(21) 2345-6789', 'arena@email.com', 'https://arenavoli.com.br', 'https://example.com/venues/arena.jpg', 1, 4.2, 85, 2, 'Seg-Sáb: 07:00-23:00', 1, 1, 1, 150.00),
    ('Centro Esportivo', 'Complexo esportivo com diversas modalidades', 'Rua dos Esportes, 300', '(31) 3456-7890', 'centro@email.com', 'https://centroesportivo.com.br', 'https://example.com/venues/centro.jpg', 1, 4.7, 210, 3, 'Seg-Dom: 06:00-00:00', 1, 1, 1, 180.00),
    ('Clube Tênis', 'Clube com quadras de tênis profissionais', 'Av. do Tênis, 400', '(41) 4567-8901', 'clube@email.com', 'https://clubetenis.com.br', 'https://example.com/venues/clube.jpg', 1, 4.8, 95, 4, 'Seg-Dom: 07:00-22:00', 1, 1, 1, 120.00),
    ('Parque Aquático', 'Complexo com piscinas olímpicas e recreativas', 'Rua das Piscinas, 500', '(51) 5678-9012', 'parque@email.com', 'https://parqueaquatico.com.br', 'https://example.com/venues/parque.jpg', 1, 4.3, 150, 5, 'Seg-Dom: 08:00-20:00', 1, 1, 1, 100.00),
    ('Pista de Corrida', 'Pista oficial para corridas e caminhadas', 'Av. da Corrida, 600', '(71) 6789-0123', 'pista@email.com', 'https://pistacorrida.com.br', 'https://example.com/venues/pista.jpg', 1, 4.0, 80, 6, 'Seg-Dom: 05:00-22:00', 1, 0, 0, 0.00),
    ('Velódromo', 'Pista de ciclismo coberta', 'Rua das Bikes, 700', '(81) 7890-1234', 'velodromo@email.com', 'https://velodromo.com.br', 'https://example.com/venues/velodromo.jpg', 1, 4.6, 70, 7, 'Seg-Sáb: 07:00-21:00', 1, 1, 1, 90.00),
    ('Estúdio Yoga', 'Espaço dedicado à prática de yoga', 'Av. da Paz, 800', '(85) 8901-2345', 'estudio@email.com', 'https://estudioyoga.com.br', 'https://example.com/venues/estudio.jpg', 1, 4.9, 200, 8, 'Seg-Sáb: 06:00-22:00', 0, 1, 1, 50.00),
    ('Box Crossfit', 'Academia especializada em crossfit', 'Rua do Treino, 900', '(62) 9012-3456', 'box@email.com', 'https://boxcrossfit.com.br', 'https://example.com/venues/box.jpg', 1, 4.4, 110, 9, 'Seg-Sex: 06:00-23:00, Sáb: 08:00-18:00', 1, 1, 1, 80.00),
    ('Praia do Surf', 'Praia com ondas perfeitas para surf', 'Av. Beira Mar, 1000', '(61) 0123-4567', 'praia@email.com', 'https://praiasurf.com.br', 'https://example.com/venues/praia.jpg', 1, 4.7, 180, 10, '24 horas', 1, 1, 0, 0.00);
GO

-- Inserir eventos
INSERT INTO Event (Title, Description, StartTime, EndTime, MaxParticipants, IsPublic, Status, SportId, CreatorId, VenueId, LocationId, ImageUrl, Price, RequiresEquipment, EquipmentDetails, SkillLevel, CreationDate, LastUpdateDate)
VALUES 
    ('Torneio de Futebol', 'Torneio amador de futebol society', '2023-07-15 09:00:00', '2023-07-15 18:00:00', 60, 1, 0, 1, 1, 1, 1, 'https://example.com/events/torneio.jpg', 50.00, 1, 'Traga sua chuteira e meião', 'Intermediário', GETDATE(), GETDATE()),
    ('Aula de Vôlei', 'Aula para iniciantes em vôlei de quadra', '2023-07-20 19:00:00', '2023-07-20 21:00:00', 20, 1, 0, 2, 2, 2, 2, 'https://example.com/events/aula.jpg', 30.00, 0, '', 'Iniciante', GETDATE(), GETDATE()),
    ('Jogo de Basquete', 'Partida amistosa de basquete 3x3', '2023-07-18 20:00:00', '2023-07-18 22:00:00', 12, 1, 0, 3, 3, 3, 3, 'https://example.com/events/jogo.jpg', 0.00, 1, 'Traga sua bola se possível', 'Todos os níveis', GETDATE(), GETDATE()),
    ('Torneio de Tênis', 'Competição de tênis em duplas', '2023-07-25 08:00:00', '2023-07-26 18:00:00', 16, 1, 0, 4, 4, 4, 4, 'https://example.com/events/tenis.jpg', 100.00, 1, 'Raquete e bolas', 'Avançado', GETDATE(), GETDATE()),
    ('Maratona Aquática', 'Competição de natação em águas abertas', '2023-08-05 07:00:00', '2023-08-05 12:00:00', 50, 1, 0, 5, 5, 5, 5, 'https://example.com/events/maratona.jpg', 80.00, 1, 'Traje de natação, óculos e touca', 'Intermediário a Avançado', GETDATE(), GETDATE()),
    ('Corrida Noturna', 'Corrida de 10km pelas ruas da cidade', '2023-07-28 20:00:00', '2023-07-28 22:00:00', 100, 1, 0, 6, 6, 6, 6, 'https://example.com/events/corrida.jpg', 40.00, 0, '', 'Todos os níveis', GETDATE(), GETDATE()),
    ('Passeio Ciclístico', 'Passeio de 30km pela orla', '2023-08-12 08:00:00', '2023-08-12 12:00:00', 50, 1, 0, 7, 7, 7, 7, 'https://example.com/events/passeio.jpg', 0.00, 1, 'Bicicleta e capacete obrigatórios', 'Todos os níveis', GETDATE(), GETDATE()),
    ('Workshop de Yoga', 'Aula especial de yoga para iniciantes', '2023-07-22 09:00:00', '2023-07-22 11:00:00', 30, 1, 0, 8, 8, 8, 8, 'https://example.com/events/workshop.jpg', 60.00, 0, 'Tapete de yoga (opcional)', 'Iniciante', GETDATE(), GETDATE()),
    ('Desafio Crossfit', 'Competição de crossfit por equipes', '2023-08-19 14:00:00', '2023-08-19 18:00:00', 40, 1, 0, 9, 9, 9, 9, 'https://example.com/events/desafio.jpg', 120.00, 0, '', 'Intermediário a Avançado', GETDATE(), GETDATE()),
    ('Aula de Surf', 'Aula para iniciantes em surf', '2023-08-06 08:00:00', '2023-08-06 11:00:00', 10, 1, 0, 10, 10, 10, 10, 'https://example.com/events/surf.jpg', 90.00, 1, 'Prancha e roupa de neoprene fornecidos', 'Iniciante', GETDATE(), GETDATE());
GO

-- Inserir relacionamentos entre usuários e esportes favoritos
INSERT INTO UserFavoriteSport (UserId, SportId)
VALUES 
    (1, 1), (1, 3), (1, 6),
    (2, 2), (2, 5), (2, 8),
    (3, 3), (3, 7), (3, 9),
    (4, 4), (4, 5), (4, 8),
    (5, 1), (5, 6), (5, 7),
    (6, 2), (6, 8), (6, 10),
    (7, 3), (7, 7), (7, 9),
    (8, 4), (8, 6), (8, 8),
    (9, 1), (9, 9), (9, 10),
    (10, 5), (10, 8), (10, 9);
GO

-- Inserir participantes nos eventos
INSERT INTO EventParticipant (EventId, UserId)
VALUES 
    (1, 1), (1, 5), (1, 9),
    (2, 2), (2, 6), (2, 10),
    (3, 3), (3, 7), (3, 1),
    (4, 4), (4, 8), (4, 2),
    (5, 5), (5, 9), (5, 3),
    (6, 6), (6, 10), (6, 4),
    (7, 7), (7, 1), (7, 5),
    (8, 8), (8, 2), (8, 6),
    (9, 9), (9, 3), (9, 7),
    (10, 10), (10, 4), (10, 8);
GO

-- Inserir relacionamentos entre locais (venues) e esportes oferecidos
INSERT INTO VenueSport (VenueId, SportId)
VALUES 
    (1, 1),
    (2, 2),
    (3, 1), (3, 2), (3, 3), (3, 4),
    (4, 4),
    (5, 5),
    (6, 6),
    (7, 7),
    (8, 8),
    (9, 9),
    (10, 10);
GO 


USE [master]
GO
/****** Object:  Database [SportSphere]    Script Date: 05/03/2025 19:27:10 ******/
CREATE DATABASE [SportSphere]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SportSphere', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SportSphere.mdf' , SIZE = 4288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SportSphere_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SportSphere_log.ldf' , SIZE = 1072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SportSphere] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SportSphere].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SportSphere] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SportSphere] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SportSphere] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SportSphere] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SportSphere] SET ARITHABORT OFF 
GO
ALTER DATABASE [SportSphere] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SportSphere] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SportSphere] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SportSphere] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SportSphere] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SportSphere] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SportSphere] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SportSphere] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SportSphere] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SportSphere] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SportSphere] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SportSphere] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SportSphere] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SportSphere] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SportSphere] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SportSphere] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SportSphere] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SportSphere] SET RECOVERY FULL 
GO
ALTER DATABASE [SportSphere] SET  MULTI_USER 
GO
ALTER DATABASE [SportSphere] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SportSphere] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SportSphere] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SportSphere] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SportSphere] SET DELAYED_DURABILITY = DISABLED 
GO
USE [SportSphere]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventParticipants]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventParticipants](
	[ParticipantsId] [int] NOT NULL,
	[ParticipatingEventsId] [int] NOT NULL,
 CONSTRAINT [PK_EventParticipants] PRIMARY KEY CLUSTERED 
(
	[ParticipantsId] ASC,
	[ParticipatingEventsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NOT NULL,
	[MaxParticipants] [int] NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[SportId] [int] NOT NULL,
	[CreatorId] [int] NOT NULL,
	[VenueId] [int] NULL,
	[LocationId] [int] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[RequiresEquipment] [bit] NOT NULL,
	[EquipmentDetails] [nvarchar](max) NOT NULL,
	[SkillLevel] [nvarchar](max) NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[LastUpdateDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[State] [nvarchar](max) NOT NULL,
	[Country] [nvarchar](max) NOT NULL,
	[PostalCode] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sports]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IconUrl] [nvarchar](max) NOT NULL,
	[RequiresEquipment] [bit] NOT NULL,
	[IsTeamSport] [bit] NOT NULL,
	[MinPlayers] [int] NOT NULL,
	[MaxPlayers] [int] NOT NULL,
	[IsPopular] [bit] NOT NULL,
 CONSTRAINT [PK_Sports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFavoriteSports]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFavoriteSports](
	[FavoriteSportsId] [int] NOT NULL,
	[PractitionersId] [int] NOT NULL,
 CONSTRAINT [PK_UserFavoriteSports] PRIMARY KEY CLUSTERED 
(
	[FavoriteSportsId] ASC,
	[PractitionersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[ProfilePictureUrl] [nvarchar](max) NOT NULL,
	[Bio] [nvarchar](max) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[LastLogin] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DefaultLocationId] [int] NOT NULL,
	[RegistrationDate] [datetime2](7) NOT NULL,
	[LastLoginDate] [datetime2](7) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Venues]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[ContactPhone] [nvarchar](max) NOT NULL,
	[ContactEmail] [nvarchar](max) NOT NULL,
	[WebsiteUrl] [nvarchar](max) NOT NULL,
	[PhotoUrl] [nvarchar](max) NOT NULL,
	[IsVerified] [bit] NOT NULL,
	[Rating] [decimal](18, 2) NOT NULL,
	[ReviewCount] [int] NOT NULL,
	[LocationId] [int] NOT NULL,
	[OpeningHours] [nvarchar](max) NOT NULL,
	[HasParking] [bit] NOT NULL,
	[HasShowers] [bit] NOT NULL,
	[HasLockers] [bit] NOT NULL,
	[PricePerHour] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Venues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VenueSports]    Script Date: 05/03/2025 19:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VenueSports](
	[OfferedSportsId] [int] NOT NULL,
	[VenuesId] [int] NOT NULL,
 CONSTRAINT [PK_VenueSports] PRIMARY KEY CLUSTERED 
(
	[OfferedSportsId] ASC,
	[VenuesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_EventParticipants_ParticipatingEventsId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_EventParticipants_ParticipatingEventsId] ON [dbo].[EventParticipants]
(
	[ParticipatingEventsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Events_CreatorId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_Events_CreatorId] ON [dbo].[Events]
(
	[CreatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Events_LocationId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Events_LocationId] ON [dbo].[Events]
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Events_SportId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_Events_SportId] ON [dbo].[Events]
(
	[SportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Events_VenueId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_Events_VenueId] ON [dbo].[Events]
(
	[VenueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserFavoriteSports_PractitionersId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_UserFavoriteSports_PractitionersId] ON [dbo].[UserFavoriteSports]
(
	[PractitionersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_DefaultLocationId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_DefaultLocationId] ON [dbo].[Users]
(
	[DefaultLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Venues_LocationId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Venues_LocationId] ON [dbo].[Venues]
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_VenueSports_VenuesId]    Script Date: 05/03/2025 19:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_VenueSports_VenuesId] ON [dbo].[VenueSports]
(
	[VenuesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventParticipants]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipants_Events_ParticipatingEventsId] FOREIGN KEY([ParticipatingEventsId])
REFERENCES [dbo].[Events] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventParticipants] CHECK CONSTRAINT [FK_EventParticipants_Events_ParticipatingEventsId]
GO
ALTER TABLE [dbo].[EventParticipants]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipants_Users_ParticipantsId] FOREIGN KEY([ParticipantsId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventParticipants] CHECK CONSTRAINT [FK_EventParticipants_Users_ParticipantsId]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Locations_LocationId]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Sports_SportId] FOREIGN KEY([SportId])
REFERENCES [dbo].[Sports] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Sports_SportId]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Users_CreatorId] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Users_CreatorId]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY([VenueId])
REFERENCES [dbo].[Venues] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Venues_VenueId]
GO
ALTER TABLE [dbo].[UserFavoriteSports]  WITH CHECK ADD  CONSTRAINT [FK_UserFavoriteSports_Sports_FavoriteSportsId] FOREIGN KEY([FavoriteSportsId])
REFERENCES [dbo].[Sports] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserFavoriteSports] CHECK CONSTRAINT [FK_UserFavoriteSports_Sports_FavoriteSportsId]
GO
ALTER TABLE [dbo].[UserFavoriteSports]  WITH CHECK ADD  CONSTRAINT [FK_UserFavoriteSports_Users_PractitionersId] FOREIGN KEY([PractitionersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserFavoriteSports] CHECK CONSTRAINT [FK_UserFavoriteSports_Users_PractitionersId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Locations_DefaultLocationId] FOREIGN KEY([DefaultLocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Locations_DefaultLocationId]
GO
ALTER TABLE [dbo].[Venues]  WITH CHECK ADD  CONSTRAINT [FK_Venues_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Venues] CHECK CONSTRAINT [FK_Venues_Locations_LocationId]
GO
ALTER TABLE [dbo].[VenueSports]  WITH CHECK ADD  CONSTRAINT [FK_VenueSports_Sports_OfferedSportsId] FOREIGN KEY([OfferedSportsId])
REFERENCES [dbo].[Sports] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VenueSports] CHECK CONSTRAINT [FK_VenueSports_Sports_OfferedSportsId]
GO
ALTER TABLE [dbo].[VenueSports]  WITH CHECK ADD  CONSTRAINT [FK_VenueSports_Venues_VenuesId] FOREIGN KEY([VenuesId])
REFERENCES [dbo].[Venues] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VenueSports] CHECK CONSTRAINT [FK_VenueSports_Venues_VenuesId]
GO
USE [master]
GO
ALTER DATABASE [SportSphere] SET  READ_WRITE 
GO
