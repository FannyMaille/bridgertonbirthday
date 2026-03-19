@echo off
echo.
echo ========================================
echo   TEST - Resume Quiz par Famille
echo ========================================
echo.

REM Insérer des réponses de test pour voir le résumé
mysql -u root -p reveensacados < test-quiz-family-summary.sql

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [OK] Donnees de test inserees !
    echo.
    echo Actions a faire :
    echo 1. Ouvrir Admin ^> Quiz
    echo 2. Voir le panneau "Resultats par Famille"
    echo 3. Verifier les scores affichees
    echo.
    echo Resultats attendus :
    echo - Sharma : 5/6 (83%%) - VERT
    echo - Bridgerton : 4/6 (67%%) - JAUNE
    echo - Hastings : 3/6 (50%%) - ORANGE
    echo - Featherington : 2/6 (33%%) - ROUGE
    echo - Danbury : 1/6 (17%%) - ROUGE
    echo.
) else (
    echo.
    echo [ERREUR] Echec de l'insertion
    echo.
)

pause
