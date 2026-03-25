@echo off
echo ================================================
echo   Integration Equipe Lady Whistledown
echo ================================================
echo.
echo Ce script va vous guider pour appliquer les modifications
echo necessaires a MonEspace.razor
echo.
echo ETAPE 1: Ouvrez le fichier MonEspace.razor
echo    Chemin: BridgertonGame.Client\Pages\MonEspace.razor
echo.
echo ETAPE 2: Dans le bloc @code, apres "private int playerPoints = 0;"
echo    Ajoutez: private int ladyWhistledownTeamPoints = 0;
echo.
echo ETAPE 3: Dans LoadPlayerData(), dans le bloc "if (currentPlayer.IsLadyWhistledown)"
echo    Apres la ligne: playerPoints = penalties.ContainsKey(...
echo    Ajoutez: ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
echo.
echo ETAPE 4: Dans PublishArticle(), dans le bloc "if (currentPlayer?.IsLadyWhistledown == true)"
echo    Apres la ligne: playerPoints = penalties.ContainsKey(...
echo    Ajoutez: ladyWhistledownTeamPoints = await ApiService.GetLadyWhistledownTeamPointsAsync();
echo.
echo ETAPE 5: Dans le HTML, apres la div "Vos points personnels" (ligne ~245)
echo    Copiez le contenu du fichier MONESPACE_TEAM_HTML.razor
echo.
echo ================================================
echo.
echo Voulez-vous ouvrir les fichiers de reference ?
echo.
choice /C YN /M "Ouvrir les fichiers (Y/N)"
if errorlevel 2 goto END
if errorlevel 1 goto OPEN

:OPEN
echo.
echo Ouverture des fichiers...
start notepad LADY_WHISTLEDOWN_TEAM_COMPLETE.md
start notepad MONESPACE_MODIFICATIONS.cs
start notepad BridgertonGame.Client\Pages\MonEspace.razor
echo.
echo Fichiers ouverts !
echo.

:END
echo ================================================
echo   Apres avoir applique les modifications,
echo   executez: dotnet build
echo   pour verifier la compilation.
echo ================================================
pause
