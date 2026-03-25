@echo off
echo ============================================
echo Recreation de la migration AddQuizSystem
echo ============================================
echo.

cd BridgertonGame.Server

echo Creation de la migration Quiz...
dotnet ef migrations add AddQuizSystem

if errorlevel 1 (
    echo.
    echo [ERREUR] La creation de la migration a echoue
    pause
    exit /b 1
)

echo.
echo [SUCCES] Migration Quiz recreee avec succes!
echo.
echo ============================================
echo Verification des migrations
echo ============================================
echo.

dotnet ef migrations list

echo.
echo ============================================
echo Instructions :
echo ============================================
echo Les 2 migrations sont maintenant presentes :
echo 1. AddQuizSystem (Quiz)
echo 2. AddChatMessages (Chat)
echo.
echo Les migrations sont deja appliquees a la BD.
echo Aucune autre action necessaire !
echo ============================================
echo.
pause
