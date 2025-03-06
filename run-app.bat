@echo off

echo SportSphere - Executar Aplicativo
echo ================================
echo 1. Executar no Windows
echo 2. Executar no Emulador Android
echo ================================
choice /c 12 /n /m "Escolha uma opcao (1-2): "

if %ERRORLEVEL% equ 1 goto windows
if %ERRORLEVEL% equ 2 goto android

:windows
echo.
echo Iniciando aplicativo no Windows...
goto docker_check

:android
echo.
echo Verificando emulador Android...
adb devices | findstr "emulator" > nul
if %ERRORLEVEL% neq 0 (
    echo Nenhum emulador Android encontrado em execucao.
    echo Deseja iniciar um emulador Android? (S/N)
    choice /c SN /n /m "Escolha uma opcao: "
    if %ERRORLEVEL% equ 1 (
        echo Iniciando emulador Android...
        start "" "emulator" -avd Pixel_3a_API_34_extension_level_7_x86_64
        echo Aguardando o emulador iniciar...
        timeout /t 30 /nobreak > nul
    ) else (
        echo Operacao cancelada.
        exit /b 1
    )
)
goto docker_check

:docker_check
echo Verificando se o Docker esta em execucao...
docker info >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Docker nao esta em execucao. Por favor, inicie o Docker antes de continuar.
    exit /b 1
)

echo Verificando se os conteineres estao em execucao...
docker-compose ps | findstr "Up" >nul
if %ERRORLEVEL% neq 0 (
    echo Os conteineres Docker nao estao em execucao. Iniciando...
    docker-compose up -d
    
    echo Aguardando os servicos iniciarem...
    timeout /t 10 /nobreak >nul
)

cd SportSphere.App

if %ERRORLEVEL% equ 1 (
    echo Executando o aplicativo MAUI no Windows...
    dotnet run --framework net9.0-windows10.0.19041.0
) else (
    echo Executando o aplicativo MAUI no Android...
    dotnet build -t:Run -f net9.0-android
)

if %ERRORLEVEL% neq 0 (
    echo Erro ao executar o aplicativo MAUI. Verifique se o .NET MAUI SDK esta instalado.
    exit /b 1
)

pause 