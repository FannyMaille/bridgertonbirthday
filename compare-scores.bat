@echo off
echo ========================================
echo   COMPARAISON POINTS CLASSEMENT VS TABLEAU
echo ========================================
echo.

SET MYSQL_PWD=Bridgerton2024!

echo Execution du diagnostic...
echo.

mysql -u bridgerton_user -D bridgerton_game < compare-scores.sql

echo.
echo ========================================
echo   Analyse terminee
echo ========================================
echo.
pause
