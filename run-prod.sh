#!/bin/bash

# Verificar se o arquivo .env existe
if [ ! -f .env ]; then
    echo "Arquivo .env não encontrado. Criando arquivo com valores padrão..."
    cat > .env << EOF
# Configurações do banco de dados
DB_PASSWORD=YourStrong!Passw0rd

# Configurações da API
API_PORT=80
API_SSL_PORT=443

# Configurações do JWT
JWT_SECRET=SportSphereSecretKey12345678901234567890
JWT_ISSUER=SportSphere
JWT_AUDIENCE=SportSphereUsers
JWT_DURATION_MINUTES=60
EOF
    echo "Arquivo .env criado. Por favor, edite-o com suas configurações antes de continuar."
    exit 1
fi

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

# Iniciar os serviços em modo de produção
echo "Iniciando os serviços do SportSphere em modo de produção..."
docker-compose -f docker-compose.prod.yml up -d

# Verificar se os serviços estão em execução
echo "Verificando o status dos serviços..."
docker-compose -f docker-compose.prod.yml ps

echo ""
echo "A API do SportSphere está disponível em: http://localhost:80 e https://localhost:443"
echo "O SQL Server está disponível em: localhost:1433"
echo ""
echo "Para parar os serviços, execute: docker-compose -f docker-compose.prod.yml down"
echo "Para visualizar os logs, execute: docker-compose -f docker-compose.prod.yml logs -f" 