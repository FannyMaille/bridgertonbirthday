@echo off
echo ============================================
echo Application de la migration ChatMessages
echo ============================================
echo.

cd BridgertonGame.Server

echo Application de la migration...
dotnet ef database update

if errorlevel 1 (
    echo.
    echo [ERREUR] L'application de la migration a echoue
    echo.
    echo Verifiez que :
    echo - MySQL est en cours d'execution
    echo - La base de donnees existe
    echo - Les parametres de connexion sont corrects
    echo.
    pause
    exit /b 1
)

echo.
echo [SUCCES] Migration appliquee avec succes!
echo.
echo La table ChatMessages a ete creee dans la base de donnees.
echo.
pause
