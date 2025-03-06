-- Verificar se o banco de dados existe, se não, criar
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SportSphere')
BEGIN
    CREATE DATABASE SportSphere;
END
GO

USE SportSphere;
GO

-- Criar tabela Sports se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sports]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Sports](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [IconUrl] [nvarchar](255) NULL,
        [RequiresEquipment] [bit] NOT NULL DEFAULT 0,
        [IsTeamSport] [bit] NOT NULL DEFAULT 0,
        [MinPlayers] [int] NULL,
        [MaxPlayers] [int] NULL,
        [IsPopular] [bit] NOT NULL DEFAULT 0
    )
END
GO

-- Criar tabela Locations se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Locations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Locations](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Address] [nvarchar](255) NOT NULL,
        [City] [nvarchar](100) NOT NULL,
        [State] [nvarchar](100) NULL,
        [Country] [nvarchar](100) NOT NULL,
        [Latitude] [float] NULL,
        [Longitude] [float] NULL
    )
END
GO

-- Criar tabela Venues se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Venues]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Venues](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [LocationId] [int] NOT NULL,
        [ContactPhone] [nvarchar](20) NULL,
        [ContactEmail] [nvarchar](100) NULL,
        [Website] [nvarchar](255) NULL,
        [Rating] [float] NULL,
        [ImageUrl] [nvarchar](255) NULL,
        CONSTRAINT [FK_Venues_Locations] FOREIGN KEY([LocationId]) REFERENCES [dbo].[Locations] ([Id])
    )
END
GO

-- Criar tabela Users se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Username] [nvarchar](50) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [PasswordHash] [nvarchar](255) NOT NULL,
        [FirstName] [nvarchar](50) NOT NULL,
        [LastName] [nvarchar](50) NOT NULL,
        [Bio] [nvarchar](500) NULL,
        [ProfilePictureUrl] [nvarchar](255) NULL,
        [Role] [nvarchar](20) NOT NULL DEFAULT 'User',
        [CreatedAt] [datetime] NOT NULL DEFAULT GETDATE(),
        [IsActive] [bit] NOT NULL DEFAULT 1
    )
END
GO

-- Criar tabela Events se não existir
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Events](
        [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [SportId] [int] NOT NULL,
        [LocationId] [int] NOT NULL,
        [VenueId] [int] NULL,
        [CreatorId] [int] NOT NULL,
        [StartTime] [datetime] NOT NULL,
        [EndTime] [datetime] NOT NULL,
        [MaxParticipants] [int] NULL,
        [IsPublic] [bit] NOT NULL DEFAULT 1,
        [Status] [int] NOT NULL DEFAULT 0,
        [ImageUrl] [nvarchar](255) NULL,
        [Price] [decimal](10, 2) NOT NULL DEFAULT 0,
        [RequiresEquipment] [bit] NOT NULL DEFAULT 0,
        [EquipmentDetails] [nvarchar](500) NULL,
        [SkillLevel] [nvarchar](50) NULL,
        [CreationDate] [datetime] NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [FK_Events_Sports] FOREIGN KEY([SportId]) REFERENCES [dbo].[Sports] ([Id]),
        CONSTRAINT [FK_Events_Locations] FOREIGN KEY([LocationId]) REFERENCES [dbo].[Locations] ([Id]),
        CONSTRAINT [FK_Events_Venues] FOREIGN KEY([VenueId]) REFERENCES [dbo].[Venues] ([Id]),
        CONSTRAINT [FK_Events_Users] FOREIGN KEY([CreatorId]) REFERENCES [dbo].[Users] ([Id])
    )
END
GO

-- Criar tabela VenueSports (relação muitos-para-muitos entre Venues e Sports)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VenueSports]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[VenueSports](
        [VenuesId] [int] NOT NULL,
        [OfferedSportsId] [int] NOT NULL,
        PRIMARY KEY ([VenuesId], [OfferedSportsId]),
        CONSTRAINT [FK_VenueSports_Venues] FOREIGN KEY([VenuesId]) REFERENCES [dbo].[Venues] ([Id]),
        CONSTRAINT [FK_VenueSports_Sports] FOREIGN KEY([OfferedSportsId]) REFERENCES [dbo].[Sports] ([Id])
    )
END
GO

-- Criar tabela UserFavoriteSports (relação muitos-para-muitos entre Users e Sports)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserFavoriteSports]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserFavoriteSports](
        [PractitionersId] [int] NOT NULL,
        [FavoriteSportsId] [int] NOT NULL,
        PRIMARY KEY ([PractitionersId], [FavoriteSportsId]),
        CONSTRAINT [FK_UserFavoriteSports_Users] FOREIGN KEY([PractitionersId]) REFERENCES [dbo].[Users] ([Id]),
        CONSTRAINT [FK_UserFavoriteSports_Sports] FOREIGN KEY([FavoriteSportsId]) REFERENCES [dbo].[Sports] ([Id])
    )
END
GO

-- Criar tabela EventParticipants (relação muitos-para-muitos entre Events e Users)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventParticipants]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[EventParticipants](
        [ParticipantsId] [int] NOT NULL,
        [ParticipatingEventsId] [int] NOT NULL,
        PRIMARY KEY ([ParticipantsId], [ParticipatingEventsId]),
        CONSTRAINT [FK_EventParticipants_Users] FOREIGN KEY([ParticipantsId]) REFERENCES [dbo].[Users] ([Id]),
        CONSTRAINT [FK_EventParticipants_Events] FOREIGN KEY([ParticipatingEventsId]) REFERENCES [dbo].[Events] ([Id])
    )
END
GO

-- Inserir esportes de exemplo (se a tabela existir)
IF OBJECT_ID('dbo.Sports', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.Sports)
    BEGIN
        INSERT INTO dbo.Sports (Name, Description, IconUrl, RequiresEquipment, IsTeamSport, MinPlayers, MaxPlayers, IsPopular)
        VALUES 
            ('Futebol', 'O esporte mais popular do mundo', 'futebol.jpg', 1, 1, 2, 22, 1),
            ('Basquete', 'Esporte de quadra com cestas', 'basquete.jpg', 1, 1, 2, 10, 1),
            ('Vôlei', 'Esporte de quadra com rede', 'volei.jpg', 1, 1, 2, 12, 1),
            ('Tênis', 'Esporte de raquete', 'tenis.jpg', 1, 0, 1, 4, 1),
            ('Natação', 'Esporte aquático', 'natacao.jpg', 0, 0, 1, 1, 1),
            ('Corrida', 'Esporte de atletismo', 'corrida.jpg', 0, 0, 1, 1, 1),
            ('Ciclismo', 'Esporte com bicicleta', 'ciclismo.jpg', 1, 0, 1, 1, 1),
            ('Yoga', 'Prática de exercícios físicos e mentais', 'yoga.jpg', 0, 0, 1, 1, 1),
            ('Musculação', 'Treinamento com pesos', 'musculacao.jpg', 1, 0, 1, 1, 1),
            ('Surf', 'Esporte aquático com prancha', 'surf.jpg', 1, 0, 1, 1, 1);
    END
END

-- Inserir localizações de exemplo (se a tabela existir)
IF OBJECT_ID('dbo.Locations', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.Locations)
    BEGIN
        INSERT INTO dbo.Locations (Address, City, State, Country, Latitude, Longitude)
        VALUES 
            ('Av. Paulista, 1000', 'São Paulo', 'SP', 'Brasil', -23.5630, -46.6543),
            ('Rua Oscar Freire, 500', 'São Paulo', 'SP', 'Brasil', -23.5616, -46.6721),
            ('Av. Atlântica, 2000', 'Rio de Janeiro', 'RJ', 'Brasil', -22.9698, -43.1866),
            ('Av. Boa Viagem, 1500', 'Recife', 'PE', 'Brasil', -8.1324, -34.9015),
            ('Av. Beira Mar, 1000', 'Fortaleza', 'CE', 'Brasil', -3.7319, -38.5267);
    END
END

-- Inserir estabelecimentos de exemplo (se a tabela existir)
IF OBJECT_ID('dbo.Venues', 'U') IS NOT NULL AND OBJECT_ID('dbo.Locations', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.Venues)
    BEGIN
        -- Assumindo que temos IDs de 1 a 5 para as localizações
        INSERT INTO dbo.Venues (Name, Description, LocationId, ContactPhone, ContactEmail, Website, Rating, ImageUrl)
        VALUES 
            ('Academia Fitness Plus', 'Academia completa com diversas modalidades', 1, '11987654321', 'contato@fitnessplus.com', 'www.fitnessplus.com', 4.5, 'academia1.jpg'),
            ('Quadra Poliesportiva Central', 'Quadras para diversos esportes', 2, '11912345678', 'contato@quadracentral.com', 'www.quadracentral.com', 4.2, 'quadra1.jpg'),
            ('Clube Aquático Ondas', 'Clube com piscinas e atividades aquáticas', 3, '21987654321', 'contato@clubeondas.com', 'www.clubeondas.com', 4.7, 'clube1.jpg'),
            ('Centro Esportivo Atletas', 'Centro de treinamento para diversos esportes', 4, '81912345678', 'contato@centroatletas.com', 'www.centroatletas.com', 4.0, 'centro1.jpg'),
            ('Estúdio Yoga e Bem-estar', 'Estúdio especializado em yoga e meditação', 5, '85987654321', 'contato@estudioyoga.com', 'www.estudioyoga.com', 4.8, 'yoga1.jpg');
    END
END

-- Inserir relação entre estabelecimentos e esportes (se a tabela existir)
IF OBJECT_ID('dbo.VenueSports', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.VenueSports)
    BEGIN
        -- Assumindo que temos IDs de 1 a 5 para os estabelecimentos e 1 a 10 para os esportes
        INSERT INTO dbo.VenueSports (VenuesId, OfferedSportsId)
        VALUES 
            (1, 5), -- Academia Fitness Plus - Natação
            (1, 9), -- Academia Fitness Plus - Musculação
            (2, 1), -- Quadra Poliesportiva Central - Futebol
            (2, 2), -- Quadra Poliesportiva Central - Basquete
            (2, 3), -- Quadra Poliesportiva Central - Vôlei
            (3, 5), -- Clube Aquático Ondas - Natação
            (3, 10), -- Clube Aquático Ondas - Surf
            (4, 1), -- Centro Esportivo Atletas - Futebol
            (4, 2), -- Centro Esportivo Atletas - Basquete
            (4, 6), -- Centro Esportivo Atletas - Corrida
            (5, 8); -- Estúdio Yoga e Bem-estar - Yoga
    END
END

-- Inserir usuários de exemplo (se a tabela existir)
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.Users)
    BEGIN
        -- Senha: 123456 (hash fictício para exemplo)
        INSERT INTO dbo.Users (Username, Email, PasswordHash, FirstName, LastName, Bio, ProfilePictureUrl, Role, CreatedAt, IsActive)
        VALUES 
            ('joaosilva', 'joao@example.com', '$2a$12$abcdefghijklmnopqrstuvwxyz123456', 'João', 'Silva', 'Entusiasta de esportes', 'joao.jpg', 'User', GETDATE(), 1),
            ('mariasantos', 'maria@example.com', '$2a$12$abcdefghijklmnopqrstuvwxyz123456', 'Maria', 'Santos', 'Atleta amadora', 'maria.jpg', 'User', GETDATE(), 1),
            ('pedrooliveira', 'pedro@example.com', '$2a$12$abcdefghijklmnopqrstuvwxyz123456', 'Pedro', 'Oliveira', 'Praticante de esportes diversos', 'pedro.jpg', 'User', GETDATE(), 1),
            ('anacosta', 'ana@example.com', '$2a$12$abcdefghijklmnopqrstuvwxyz123456', 'Ana', 'Costa', 'Instrutora de yoga', 'ana.jpg', 'User', GETDATE(), 1),
            ('admin', 'admin@sportsphere.com', '$2a$12$abcdefghijklmnopqrstuvwxyz123456', 'Admin', 'System', 'Administrador do sistema', 'admin.jpg', 'Admin', GETDATE(), 1);
    END
END

-- Inserir relação entre usuários e esportes favoritos (se a tabela existir)
IF OBJECT_ID('dbo.UserFavoriteSports', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.UserFavoriteSports)
    BEGIN
        -- Assumindo que temos IDs de 1 a 5 para os usuários e 1 a 10 para os esportes
        INSERT INTO dbo.UserFavoriteSports (PractitionersId, FavoriteSportsId)
        VALUES 
            (1, 1), -- João - Futebol
            (1, 2), -- João - Basquete
            (2, 3), -- Maria - Vôlei
            (2, 5), -- Maria - Natação
            (3, 1), -- Pedro - Futebol
            (3, 7), -- Pedro - Ciclismo
            (4, 8), -- Ana - Yoga
            (4, 9); -- Ana - Musculação
    END
END

-- Inserir eventos de exemplo (se a tabela existir)
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.Events)
    BEGIN
        -- Assumindo que temos IDs de 1 a 5 para os usuários, 1 a 5 para as localizações e 1 a 10 para os esportes
        INSERT INTO dbo.Events (Title, Description, SportId, LocationId, VenueId, CreatorId, StartTime, EndTime, MaxParticipants, IsPublic, Status, ImageUrl, Price, RequiresEquipment, EquipmentDetails, SkillLevel, CreationDate)
        VALUES 
            ('Partida de Futebol Amistosa', 'Venha jogar futebol com a gente!', 1, 1, NULL, 1, DATEADD(DAY, 7, GETDATE()), DATEADD(DAY, 7, DATEADD(HOUR, 2, GETDATE())), 22, 1, 0, 'evento1.jpg', 0, 1, 'Traga sua chuteira', 'Iniciante', GETDATE()),
            ('Aula de Yoga para Iniciantes', 'Aula introdutória de yoga', 8, 5, 5, 4, DATEADD(DAY, 3, GETDATE()), DATEADD(DAY, 3, DATEADD(HOUR, 1, GETDATE())), 15, 1, 0, 'evento2.jpg', 25, 0, '', 'Iniciante', GETDATE()),
            ('Treino de Basquete', 'Treino técnico e tático de basquete', 2, 2, 2, 1, DATEADD(DAY, 5, GETDATE()), DATEADD(DAY, 5, DATEADD(HOUR, 2, GETDATE())), 10, 1, 0, 'evento3.jpg', 0, 1, 'Traga sua bola', 'Intermediário', GETDATE()),
            ('Natação para Todos', 'Aula de natação para todos os níveis', 5, 3, 3, 2, DATEADD(DAY, 4, GETDATE()), DATEADD(DAY, 4, DATEADD(HOUR, 1, GETDATE())), 20, 1, 0, 'evento4.jpg', 30, 1, 'Traga sua touca e óculos', 'Todos os níveis', GETDATE()),
            ('Passeio de Bicicleta', 'Passeio de bicicleta pela cidade', 7, 4, NULL, 3, DATEADD(DAY, 10, GETDATE()), DATEADD(DAY, 10, DATEADD(HOUR, 3, GETDATE())), 30, 1, 0, 'evento5.jpg', 0, 1, 'Traga sua bicicleta e capacete', 'Todos os níveis', GETDATE());
    END
END

-- Inserir participantes de eventos (se a tabela existir)
IF OBJECT_ID('dbo.EventParticipants', 'U') IS NOT NULL
BEGIN
    -- Verificar se já existem dados
    IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.EventParticipants)
    BEGIN
        -- Assumindo que temos IDs de 1 a 5 para os eventos e 1 a 5 para os usuários
        -- O criador do evento já é um participante por padrão
        INSERT INTO dbo.EventParticipants (ParticipantsId, ParticipatingEventsId)
        VALUES 
            (1, 1), -- João já é criador do evento 1
            (2, 1), -- Maria participa do evento 1
            (3, 1), -- Pedro participa do evento 1
            (4, 2), -- Ana já é criadora do evento 2
            (2, 2), -- Maria participa do evento 2
            (1, 3), -- João já é criador do evento 3
            (3, 3), -- Pedro participa do evento 3
            (2, 4), -- Maria já é criadora do evento 4
            (4, 4), -- Ana participa do evento 4
            (3, 5), -- Pedro já é criador do evento 5
            (1, 5); -- João participa do evento 5
    END
END

-- Adicionar mais eventos para melhorar a experiência do usuário
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL
BEGIN
    -- Adicionar mais eventos apenas se tivermos os 5 eventos básicos
    IF (SELECT COUNT(*) FROM dbo.Events) = 5
    BEGIN
        INSERT INTO dbo.Events (Title, Description, SportId, LocationId, VenueId, CreatorId, StartTime, EndTime, MaxParticipants, IsPublic, Status, ImageUrl, Price, RequiresEquipment, EquipmentDetails, SkillLevel, CreationDate)
        VALUES 
            ('Torneio de Tênis', 'Torneio amador de tênis', 4, 2, 2, 2, DATEADD(DAY, 14, GETDATE()), DATEADD(DAY, 14, DATEADD(HOUR, 4, GETDATE())), 16, 1, 0, 'evento6.jpg', 50, 1, 'Traga sua raquete', 'Intermediário', GETDATE()),
            ('Aula de Musculação', 'Aula de musculação para iniciantes', 9, 1, 1, 3, DATEADD(DAY, 2, GETDATE()), DATEADD(DAY, 2, DATEADD(HOUR, 1, GETDATE())), 10, 1, 0, 'evento7.jpg', 20, 0, '', 'Iniciante', GETDATE()),
            ('Corrida Matinal', 'Corrida em grupo pelo parque', 6, 4, NULL, 4, DATEADD(DAY, 1, GETDATE()), DATEADD(DAY, 1, DATEADD(HOUR, 1, GETDATE())), 20, 1, 0, 'evento8.jpg', 0, 1, 'Traga água e roupas confortáveis', 'Todos os níveis', GETDATE()),
            ('Surf para Iniciantes', 'Aula de surf para quem está começando', 10, 3, 3, 1, DATEADD(DAY, 8, GETDATE()), DATEADD(DAY, 8, DATEADD(HOUR, 2, GETDATE())), 8, 1, 0, 'evento9.jpg', 80, 1, 'Equipamentos fornecidos', 'Iniciante', GETDATE()),
            ('Jogo de Vôlei', 'Partida amistosa de vôlei', 3, 2, 2, 2, DATEADD(DAY, 6, GETDATE()), DATEADD(DAY, 6, DATEADD(HOUR, 2, GETDATE())), 12, 1, 0, 'evento10.jpg', 0, 0, '', 'Todos os níveis', GETDATE());
            
        -- Adicionar participantes para os novos eventos
        INSERT INTO dbo.EventParticipants (ParticipantsId, ParticipatingEventsId)
        VALUES 
            (2, 6), -- Maria já é criadora do evento 6
            (1, 6), -- João participa do evento 6
            (3, 7), -- Pedro já é criador do evento 7
            (2, 7), -- Maria participa do evento 7
            (4, 8), -- Ana já é criadora do evento 8
            (3, 8), -- Pedro participa do evento 8
            (1, 9), -- João já é criador do evento 9
            (4, 9), -- Ana participa do evento 9
            (2, 10), -- Maria já é criadora do evento 10
            (1, 10), -- João participa do evento 10
            (3, 10); -- Pedro participa do evento 10
    END
END

PRINT 'Dados de exemplo inseridos com sucesso!';
GO 