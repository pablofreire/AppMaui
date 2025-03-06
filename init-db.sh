#!/bin/bash

# Esperar pelo SQL Server iniciar
echo "Aguardando SQL Server iniciar..."
sleep 30s

# Executar migrações usando sqlcmd com TrustServerCertificate
echo "Executando migrações do SQL..."
/opt/mssql-tools18/bin/sqlcmd -S "db" -U sa -P "${DB_PASSWORD}" -d master -i /app/migrations.sql -C -N -t 30 -b

echo "Banco de dados inicializado com sucesso!" 