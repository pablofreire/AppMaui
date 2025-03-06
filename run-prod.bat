@echo off

REM Verificar se o arquivo .env existe
if not exist .env (
    echo Arquivo .env nao encontrado. Criando arquivo com valores padrao...
    (
        echo # Configuracoes do banco de dados
        echo DB_PASSWORD=YourStrong!Passw0rd
        echo.
        echo # Configuracoes da API
        echo API_PORT=80
        echo API_SSL_PORT=443
        echo.
        echo # Configuracoes do JWT
        echo JWT_SECRET=SportSphereSecretKey12345678901234567890
        echo JWT_ISSUER=SportSphere
        echo JWT_AUDIENCE=SportSphereUsers
        echo JWT_DURATION_MINUTES=60
    ) > .env
    echo Arquivo .env criado. Por favor, edite-o com suas configuracoes antes de continuar.
    exit /b 1
)

REM Verificar se o Docker está instalado
where docker >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo Docker nao esta instalado. Por favor, instale o Docker antes de continuar.
    exit /b 1
)

REM Verificar se o Docker Compose está instalado
where docker-compose >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo Docker Compose nao esta instalado. Por favor, instale o Docker Compose antes de continuar.
    exit /b 1
)

REM Iniciar os serviços em modo de produção
echo Iniciando os servicos do SportSphere em modo de producao...
docker-compose -f docker-compose.prod.yml up -d

REM Verificar se os serviços estão em execução
echo Verificando o status dos servicos...
docker-compose -f docker-compose.prod.yml ps

echo.
echo A API do SportSphere esta disponivel em: http://localhost:80 e https://localhost:443
echo O SQL Server esta disponivel em: localhost:1433
echo.
echo Para parar os servicos, execute: docker-compose -f docker-compose.prod.yml down
echo Para visualizar os logs, execute: docker-compose -f docker-compose.prod.yml logs -f

pause 