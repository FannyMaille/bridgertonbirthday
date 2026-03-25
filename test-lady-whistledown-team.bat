@echo off
echo ================================================
echo   TEST - Equipe Lady Whistledown
echo ================================================
echo.

echo Verification du build...
dotnet build --no-restore
if %errorlevel% neq 0 (
    echo [ERREUR] Le build a echoue
    pause
    exit /b 1
)
echo [OK] Build reussi
echo.

echo ================================================
echo   TESTS A EFFECTUER MANUELLEMENT
echo ================================================
echo.
echo 1. Lancez l'application :
echo    cd BridgertonGame.Server
echo    dotnet run
echo.
echo 2. Ouvrez un navigateur sur http://localhost:5000
echo.
echo 3. TEST DU CLASSEMENT :
echo    - Allez sur /classement
echo    - Verifiez que l'equipe Lady Whistledown apparait
echo    - Elle doit avoir un design rose distinctif
echo    - Son rang doit etre calcule avec les familles
echo.
echo 4. TEST DE PUBLICATION :
echo    - Connectez-vous comme Lady Whistledown
echo    - Publiez un article
echo    - Retournez sur /classement
echo    - Les points de l'equipe doivent augmenter
echo    - Le rang peut changer
echo.
echo 5. TEST DE MON ESPACE (apres modifications) :
echo    - Connectez-vous comme Lady Whistledown
echo    - Verifiez que 2 cartes de points apparaissent :
echo      * Vos points personnels (violet)
echo      * Equipe Lady Whistledown (rose)
echo.
echo ================================================
echo   CHECKLIST DE VERIFICATION
echo ================================================
echo.
echo [ ] Backend fonctionne
echo [ ] API repond correctement
echo [ ] Classement affiche l'equipe LW
echo [ ] Design rose est applique
echo [ ] Rang se calcule correctement
echo [ ] Publication met a jour les points
echo [ ] MonEspace affiche les 2 cartes
echo.
echo ================================================
pause
