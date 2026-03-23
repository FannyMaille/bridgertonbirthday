@echo off
echo ========================================
echo    Test du Systeme de Quiz
echo ========================================
echo.

echo [1/5] Verification de la migration...
cd BridgertonGame.Server
dotnet ef migrations list | findstr "AddQuizSystem"
if %errorlevel% == 0 (
    echo [OK] Migration AddQuizSystem trouvee
) else (
    echo [ERREUR] Migration AddQuizSystem introuvable
    pause
    exit /b 1
)
echo.

echo [2/5] Verification des tables...
mysql -u root -p"y4!xM6kzk66pq#j$;" -D bridgerton -e "SHOW TABLES LIKE 'Quiz%%';"
if %errorlevel% == 0 (
    echo [OK] Tables Quiz creees
) else (
    echo [ERREUR] Tables Quiz introuvables
)
echo.

echo [3/5] Verification de l'etat du quiz...
mysql -u root -p"y4!xM6kzk66pq#j$;" -D bridgerton -e "SELECT * FROM QuizStates;"
echo.

echo [4/5] Verification des questions...
mysql -u root -p"y4!xM6kzk66pq#j$;" -D bridgerton -e "SELECT COUNT(*) as NombreQuestions FROM Quizzes;"
echo.

echo [5/5] Verification des reponses...
mysql -u root -p"y4!xM6kzk66pq#j$;" -D bridgerton -e "SELECT COUNT(*) as NombreReponses FROM QuizAnswers;"
echo.

echo ========================================
echo    Test termine !
echo ========================================
echo.
echo Prochaines etapes :
echo 1. Connectez-vous a l'admin
echo 2. Allez dans l'onglet Quiz
echo 3. Creez votre premiere question
echo 4. Activez le quiz
echo.

pause
