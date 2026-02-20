@echo off
echo ========================================
echo Migration vers Base de Donnees SQLite
echo ========================================
echo.

cd BridgertonGame.Server

echo Etape 1: Nettoyage...
if exist bridgerton.db del bridgerton.db
if exist bridgerton.db-shm del bridgerton.db-shm
if exist bridgerton.db-wal del bridgerton.db-wal

echo.
echo Etape 2: Build du projet...
dotnet build

echo.
echo Etape 3: Application des migrations...
dotnet ef database update

echo.
echo ========================================
echo Migration terminee !
echo ========================================
echo.
echo La base de donnees bridgerton.db a ete creee.
echo Vous pouvez maintenant demarrer le serveur avec start-server.bat
echo.

pause
