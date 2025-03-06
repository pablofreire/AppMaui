#!/bin/bash

# Verificar se o Docker está em execução
if ! docker info > /dev/null 2>&1; then
    echo "Docker não está em execução. Por favor, inicie o Docker antes de continuar."
    exit 1
fi

# Verificar se os contêineres estão em execução
if ! docker-compose ps | grep -q "Up"; then
    echo "Os contêineres Docker não estão em execução. Iniciando..."
    docker-compose up -d
    
    # Aguardar um pouco para os serviços iniciarem
    echo "Aguardando os serviços iniciarem..."
    sleep 10s
fi

# Executar o aplicativo MAUI
echo "Executando o aplicativo MAUI..."
cd SportSphere.App
dotnet run

if [ $? -ne 0 ]; then
    echo "Erro ao executar o aplicativo MAUI. Verifique se o .NET MAUI SDK está instalado."
    exit 1
fi 