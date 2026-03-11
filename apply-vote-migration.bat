@echo off
echo ===================================
echo Applying Vote Migration
echo ===================================
echo.

cd BridgertonGame.Server

echo Updating database...
dotnet ef database update

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Failed to update database
    pause
    exit /b 1
)

echo.
echo ==============================================
echo Vote system migration applied successfully!
echo ==============================================
echo.
echo The following tables have been added:
echo - Votes: Stores user votes for Lady Whistledown
echo - VoteResults: Stores calculated results when revealed
echo.
echo You can now:
echo 1. Enable voting for families in the Admin panel
echo 2. Players can vote for Lady Whistledown
echo 3. Reveal results to automatically award points
echo.
pause
