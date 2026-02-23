@echo off
echo ========================================
echo Vider le cache du navigateur
echo ========================================
echo.
echo Les donnees sont maintenant rechargees automatiquement !
echo.
echo Mais si vous voulez vider manuellement le cache :
echo.
echo 1. Ouvrez votre navigateur sur http://localhost:5177
echo 2. Appuyez sur F12 (Outils developpeur)
echo 3. Application ^> Local Storage ^> http://localhost:5177
echo 4. Supprimez les cles "bridgerton_currentPlayer" et "bridgerton_isAdmin"
echo 5. Rechargez la page (F5)
echo.
echo OU PLUS SIMPLE :
echo 1. Allez sur http://localhost:5177/mon-espace
echo 2. Cliquez sur "Deconnexion"
echo 3. Reconnectez-vous avec votre code
echo.
echo ========================================
echo Les modifications ont ete appliquees !
echo ========================================
echo.
echo Maintenant, a chaque chargement de "Mon Espace",
echo les donnees sont TOUJOURS rechargees depuis la BDD !
echo.
pause
