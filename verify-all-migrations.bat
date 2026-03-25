@echo off
echo ============================================
echo Verification des migrations appliquees
echo ============================================
echo.

cd BridgertonGame.Server

echo Liste de toutes les migrations :
dotnet ef migrations list

echo.
echo ============================================
echo Verification des tables en base de donnees
echo ============================================
echo.

mysql -u root -py4!xM6kzk66pq#j$ -D bridgerton -e "SHOW TABLES LIKE 'Quiz%%';"
mysql -u root -py4!xM6kzk66pq#j$ -D bridgerton -e "SHOW TABLES LIKE 'Chat%%';"

echo.
echo [VERIFICATION COMPLETE]
echo.
pause
