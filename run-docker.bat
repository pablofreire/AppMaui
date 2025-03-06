@echo off
echo Verificando se o Docker esta instalado...
where docker >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo Docker nao esta instalado. Por favor, instale o Docker antes de continuar.
    exit /b 1
)

echo Verificando se o Docker Compose esta instalado...
where docker-compose >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo Docker Compose nao esta instalado. Por favor, instale o Docker Compose antes de continuar.
    exit /b 1
)

echo Iniciando os servicos do SportSphere...
docker-compose up -d

echo Verificando o status dos servicos...
docker-compose ps

echo.
echo A API do SportSphere esta disponivel em: http://localhost:5000
echo O SQL Server esta disponivel em: localhost:1433
echo.
echo Para parar os servicos, execute: docker-compose down
echo Para visualizar os logs, execute: docker-compose logs -f

pause 