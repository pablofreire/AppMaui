#!/bin/bash

# Verificar se o Docker está instalado
if ! command -v docker &> /dev/null; then
    echo "Docker não está instalado. Por favor, instale o Docker antes de continuar."
    exit 1
fi

# Verificar se o Docker Compose está instalado
if ! command -v docker-compose &> /dev/null; then
    echo "Docker Compose não está instalado. Por favor, instale o Docker Compose antes de continuar."
    exit 1
fi

# Iniciar os serviços
echo "Iniciando os serviços do SportSphere..."
docker-compose up -d

# Verificar se os serviços estão em execução
echo "Verificando o status dos serviços..."
docker-compose ps

echo ""
echo "A API do SportSphere está disponível em: http://localhost:5000"
echo "O SQL Server está disponível em: localhost:1433"
echo ""
echo "Para parar os serviços, execute: docker-compose down"
echo "Para visualizar os logs, execute: docker-compose logs -f" 