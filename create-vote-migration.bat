@echo off
echo ===================================
echo Migration: Add Vote System
echo ===================================
echo.

cd BridgertonGame.Server

echo Creating migration...
dotnet ef migrations add AddVoteSystem

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Failed to create migration
    pause
    exit /b 1
)

echo.
echo Migration created successfully!
echo.
echo Next steps:
echo 1. Review the migration file in Migrations folder
echo 2. Run apply-vote-migration.bat to apply the migration
echo.
pause
