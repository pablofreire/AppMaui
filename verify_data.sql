-- Script para verificar os dados inseridos no banco de dados SportSphere
USE SportSphere;
GO

-- Verificar quantidade de registros em cada tabela
PRINT '=== VERIFICAÇÃO DE DADOS ==='
PRINT ''

PRINT '=== CONTAGEM DE REGISTROS ==='
SELECT 'Location' AS Tabela, COUNT(*) AS Registros FROM Location
UNION ALL
SELECT 'Sport', COUNT(*) FROM Sport
UNION ALL
SELECT 'User', COUNT(*) FROM [User]
UNION ALL
SELECT 'Venue', COUNT(*) FROM Venue
UNION ALL
SELECT 'Event', COUNT(*) FROM Event
UNION ALL
SELECT 'UserFavoriteSport', COUNT(*) FROM UserFavoriteSport
UNION ALL
SELECT 'EventParticipant', COUNT(*) FROM EventParticipant
UNION ALL
SELECT 'VenueSport', COUNT(*) FROM VenueSport;
GO

-- Verificar dados de usuários
PRINT '=== USUÁRIOS ==='
SELECT Id, Username, Email, FirstName, LastName, Role FROM [User];
GO

-- Verificar dados de esportes
PRINT '=== ESPORTES ==='
SELECT Id, Name, IsTeamSport, MinPlayers, MaxPlayers, IsPopular FROM Sport;
GO

-- Verificar dados de eventos
PRINT '=== EVENTOS ==='
SELECT e.Id, e.Title, e.StartTime, e.EndTime, e.MaxParticipants, 
       s.Name AS Sport, u.Username AS Creator, v.Name AS Venue
FROM Event e
JOIN Sport s ON e.SportId = s.Id
JOIN [User] u ON e.CreatorId = u.Id
LEFT JOIN Venue v ON e.VenueId = v.Id;
GO

-- Verificar relacionamentos entre usuários e esportes favoritos
PRINT '=== ESPORTES FAVORITOS POR USUÁRIO ==='
SELECT u.Username, STRING_AGG(s.Name, ', ') AS FavoriteSports
FROM [User] u
JOIN UserFavoriteSport ufs ON u.Id = ufs.UserId
JOIN Sport s ON ufs.SportId = s.Id
GROUP BY u.Username;
GO

-- Verificar participantes de eventos
PRINT '=== PARTICIPANTES POR EVENTO ==='
SELECT e.Title, STRING_AGG(u.Username, ', ') AS Participants
FROM Event e
JOIN EventParticipant ep ON e.Id = ep.EventId
JOIN [User] u ON ep.UserId = u.Id
GROUP BY e.Title;
GO

-- Verificar esportes oferecidos por local
PRINT '=== ESPORTES POR LOCAL ==='
SELECT v.Name, STRING_AGG(s.Name, ', ') AS OfferedSports
FROM Venue v
JOIN VenueSport vs ON v.Id = vs.VenueId
JOIN Sport s ON vs.SportId = s.Id
GROUP BY v.Name;
GO

PRINT '=== VERIFICAÇÃO CONCLUÍDA ==='
GO 