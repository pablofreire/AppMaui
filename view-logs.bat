@echo off

if "%1"=="api" (
    echo Visualizando logs do conteiner da API...
    docker-compose logs -f api
) else if "%1"=="db" (
    echo Visualizando logs do conteiner do banco de dados...
    docker-compose logs -f db
) else (
    echo Visualizando logs de todos os conteineres...
    docker-compose logs -f
) 