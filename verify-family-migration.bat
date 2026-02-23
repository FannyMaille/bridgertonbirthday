@echo off
echo ========================================
echo Verification de la migration FamilyId
echo ========================================
echo.

mysql -u root -py4!xM6kzk66pq#j$ -e "USE bridgerton; DESCRIBE Players;" | findstr "FamilyId"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Colonne FamilyId trouvee dans la table Players
    echo Verifiez si "Null" est "YES" dans la ligne ci-dessus
) else (
    echo.
    echo [ERREUR] Colonne FamilyId non trouvee !
)

echo.
echo ========================================
echo Pour appliquer la migration :
echo create-nullable-family-migration.bat
echo ========================================
echo.
pause
