@echo off
echo ========================================
echo Test des Notifications
echo ========================================
echo.

echo Choix du test:
echo 1 - Notification d'article
echo 2 - Notification personnalisee
echo 3 - Notification de succes
echo 4 - Notification d'avertissement
echo.

set /p choice="Votre choix (1-4): "

if "%choice%"=="1" (
    echo.
    echo Envoi d'une notification d'article...
    curl -X POST "https://localhost:7113/api/notifications/article-test" -H "Content-Type: application/json" -k
    goto end
)

if "%choice%"=="2" (
    set /p title="Titre: "
    set /p message="Message: "
    echo.
    echo Envoi de la notification...
    curl -X POST "https://localhost:7113/api/notifications/test" -H "Content-Type: application/json" -d "{\"title\":\"%title%\",\"message\":\"%message%\",\"type\":\"info\"}" -k
    goto end
)

if "%choice%"=="3" (
    echo.
    echo Envoi d'une notification de succes...
    curl -X POST "https://localhost:7113/api/notifications/test" -H "Content-Type: application/json" -d "{\"title\":\"✅ Succès\",\"message\":\"Opération réussie avec succès\",\"type\":\"success\"}" -k
    goto end
)

if "%choice%"=="4" (
    echo.
    echo Envoi d'une notification d'avertissement...
    curl -X POST "https://localhost:7113/api/notifications/test" -H "Content-Type: application/json" -d "{\"title\":\"⚠️ Attention\",\"message\":\"Ceci est un avertissement important\",\"type\":\"warning\"}" -k
    goto end
)

echo Choix invalide!

:end
echo.
echo ========================================
echo Notification envoyee!
echo Verifiez dans votre navigateur.
echo ========================================
pause
