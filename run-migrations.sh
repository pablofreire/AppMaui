#!/bin/bash

echo "Executando migrações do Entity Framework Core no contêiner Docker..."

# Entrar no contêiner da API
docker-compose exec api bash -c "cd /src/SportSphere.API && dotnet ef database update"

if [ $? -eq 0 ]; then
    echo "Migrações aplicadas com sucesso!"
else
    echo "Erro ao aplicar migrações. Verifique se o contêiner da API está em execução."
    exit 1
fi 