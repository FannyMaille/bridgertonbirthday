@echo off
echo ============================================
echo Creation de la migration ChatMessages
echo ============================================
echo.

cd BridgertonGame.Server

echo Suppression des anciennes migrations Chat si elles existent...
dotnet ef migrations remove --force 2>nul

echo.
echo Creation de la nouvelle migration...
dotnet ef migrations add AddChatMessages

if errorlevel 1 (
    echo.
    echo [ERREUR] La creation de la migration a echoue
    pause
    exit /b 1
)

echo.
echo [SUCCES] Migration creee avec succes!
echo.
echo ============================================
echo Instructions :
echo ============================================
echo 1. Verifiez la migration generee dans le dossier Migrations
echo 2. Lancez apply-chat-migration.bat pour appliquer la migration
echo ============================================
echo.
pause
