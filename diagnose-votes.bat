@echo off
echo ========================================
echo Diagnostic des Votes - Famille Bridgerton
echo ========================================
echo.

cd BridgertonGame.Server

echo Execution du diagnostic SQL...
mysql -h localhost -P 3306 -u root -p -D bridgerton < ..\diagnose-votes.sql

echo.
echo ========================================
echo Diagnostic termine !
echo ========================================
echo.
echo Verifiez les resultats ci-dessus :
echo.
echo 1. FAMILLE : VotingEnabled et Revealed doivent etre a 1
echo 2. VOTES : Vous devriez voir le vote d'Isabelle pour Julien
echo 3. VOTERESULTS : Apparait seulement si Revealed = 1
echo 4. GAMESCORES : Les points de votes apparaissent si Revealed = 1
echo.
pause
