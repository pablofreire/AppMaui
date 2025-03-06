@echo off
echo Executando migracoes do Entity Framework Core no conteiner Docker...

docker-compose exec api bash -c "cd /src/SportSphere.API && dotnet ef database update"

if %ERRORLEVEL% neq 0 (
    echo Erro ao aplicar migracoes. Verifique se o conteiner da API esta em execucao.
    exit /b 1
) else (
    echo Migracoes aplicadas com sucesso!
)

pause 