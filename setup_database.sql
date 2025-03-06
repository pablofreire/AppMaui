-- Script para configurar o banco de dados SportSphere
-- Este script executa em sequência:
-- 1. Criação do esquema do banco de dados
-- 2. Inserção dos dados iniciais

-- Executar o script de criação do esquema
:r database_schema.sql
GO

-- Executar o script de inserção de dados
:r initial_data.sql
GO

PRINT 'Configuração do banco de dados SportSphere concluída com sucesso!'
GO 