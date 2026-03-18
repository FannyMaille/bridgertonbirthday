@echo off
echo ========================================
echo  Insertion de Questions de Test - Quiz
echo ========================================
echo.
echo Ce script va ajouter 5 questions de test
echo dans votre systeme de quiz Bridgerton.
echo.
echo Questions qui seront ajoutees :
echo 1. Dans quelle famille Penelope est-elle née ?
echo 2. Qui est la mystérieuse Lady Whistledown ?
echo 3. Dans quelle ville se déroule l'histoire ?
echo 4. Combien d'enfants Bridgerton y a-t-il ?
echo 5. Comment s'appelle la reine ?
echo.
pause
echo.

echo Insertion des questions...
mysql -u root -p"y4!xM6kzk66pq#j$;" -D bridgerton < insert-quiz-test-questions.sql

if %errorlevel% == 0 (
    echo.
    echo ========================================
    echo [SUCCESS] Questions inserees !
    echo ========================================
    echo.
    echo Prochaines etapes :
    echo 1. Connectez-vous a l'interface Admin
    echo 2. Allez dans l'onglet Quiz
    echo 3. Vous devriez voir les 5 questions
    echo 4. Activez le quiz
    echo 5. Selectionnez la Question 1
    echo 6. Les joueurs peuvent maintenant repondre !
    echo.
) else (
    echo.
    echo ========================================
    echo [ERREUR] Probleme lors de l'insertion
    echo ========================================
    echo.
    echo Verifiez :
    echo - Que MySQL est demarré
    echo - Que le mot de passe est correct
    echo - Que la base "bridgerton" existe
    echo.
)

pause
