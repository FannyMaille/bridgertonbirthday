@echo off
echo ========================================
echo Synchronisation Lady Whistledown
echo ========================================
echo.

REM Récupérer les paramètres de connexion depuis appsettings.json
set "CONFIG_FILE=BridgertonGame.Server\appsettings.json"

REM Lire la chaîne de connexion (version simplifiée)
for /f "tokens=2 delims=:," %%a in ('findstr /C:"DefaultConnection" %CONFIG_FILE%') do set "CONN_STRING=%%a"
set CONN_STRING=%CONN_STRING:"=%
set CONN_STRING=%CONN_STRING: =%

REM Extraire les composants de la chaîne de connexion
for /f "tokens=1-4 delims=;" %%a in ("%CONN_STRING%") do (
    set "PART1=%%a"
    set "PART2=%%b"
    set "PART3=%%c"
    set "PART4=%%d"
)

REM Analyser les parties
for /f "tokens=2 delims==" %%a in ("%PART1%") do set "SERVER=%%a"
for /f "tokens=2 delims==" %%a in ("%PART2%") do set "DATABASE=%%a"
for /f "tokens=2 delims==" %%a in ("%PART3%") do set "USER=%%a"
for /f "tokens=2 delims==" %%a in ("%PART4%") do set "PASSWORD=%%a"

echo Synchronisation des rôles Lady Whistledown...
echo.

mysql -h %SERVER% -u %USER% -p%PASSWORD% %DATABASE% < sync-lady-whistledown.sql

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo ✓ Synchronisation réussie !
    echo ========================================
    echo.
    echo Les rôles Lady Whistledown ont été synchronisés entre :
    echo - Players.IsLadyWhistledown
    echo - Players.Role
    echo - Families.LadyWhistledownId
    echo.
) else (
    echo.
    echo ========================================
    echo ✗ Erreur lors de la synchronisation
    echo ========================================
    echo.
)

pause
