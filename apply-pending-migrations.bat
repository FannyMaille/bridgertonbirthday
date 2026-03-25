@echo off
echo ============================================
echo Application de la migration Quiz
echo ============================================
echo.

cd BridgertonGame.Server

echo Application de toutes les migrations en attente...
dotnet ef database update

if errorlevel 1 (
    echo.
    echo [ERREUR] L'application de la migration a echoue
    pause
    exit /b 1
)

echo.
echo [SUCCES] Migration appliquee avec succes!
echo.
pause
