@echo off
echo Parando e removendo conteineres Docker do SportSphere...
docker-compose down

echo Removendo volumes Docker do SportSphere...
docker-compose down -v

echo Removendo imagens Docker do SportSphere...
for /f "tokens=3" %%i in ('docker images ^| findstr "sportsphere"') do (
    docker rmi %%i
)

echo Limpeza concluida!
pause 