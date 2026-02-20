@echo off
echo ========================================
echo Migration : FamilyId nullable
echo ========================================
echo.

cd BridgertonGame.Server

echo Creation de la migration...
dotnet ef migrations add MakeFamilyIdNullable

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERREUR] La creation de la migration a echoue !
    pause
    exit /b 1
)

echo.
echo Application de la migration...
dotnet ef database update

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERREUR] L'application de la migration a echoue !
    pause
    exit /b 1
)

echo.
echo ========================================
echo Migration appliquee avec succes !
echo ========================================
echo.
echo La colonne FamilyId est maintenant nullable.
echo Les Maitresses de maison peuvent maintenant
echo etre creees sans famille associee.
echo.
pause
