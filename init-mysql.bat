@echo off
echo ========================================
echo Initialisation Base de Donnees MySQL
echo ========================================
echo.

SET MYSQL_HOST=localhost
SET MYSQL_PORT=3306
SET MYSQL_USER=root
SET MYSQL_PASSWORD=y4!xM6kzk66pq#j$

echo Etape 1: Verification de MySQL...
mysql --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERREUR: MySQL n'est pas installe ou n'est pas dans le PATH
    echo Veuillez installer MySQL ou ajouter mysql.exe au PATH
    pause
    exit /b 1
)

echo.
echo Etape 2: Connexion a MySQL et execution du script...
mysql -h %MYSQL_HOST% -P %MYSQL_PORT% -u %MYSQL_USER% -p%MYSQL_PASSWORD% < BridgertonGame.Server\mysql-init.sql

if %errorlevel% neq 0 (
    echo.
    echo ERREUR: Impossible d'executer le script SQL
    echo Verifiez:
    echo   1. Que MySQL est demarre
    echo   2. Que le mot de passe est correct
    echo   3. Que l'utilisateur root a les droits necessaires
    pause
    exit /b 1
)

echo.
echo ========================================
echo Base de donnees initialisee avec succes !
echo ========================================
echo.
echo La base 'bridgerton' a ete creee avec toutes les tables et donnees.
echo Vous pouvez maintenant demarrer le serveur avec start-server.bat
echo.

pause
