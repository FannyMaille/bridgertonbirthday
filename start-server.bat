@echo off
echo ===========================================
echo Starting BridgertonGame Server (will build and run)...
echo ===========================================

:: Change to repository root based on this script location and start server project
pushd "%~dp0"

:: Start a new cmd window that trusts dev certs (silently) and runs the server on the expected URLs
start "BridgertonGame Server" cmd /k "cd /d "%~dp0BridgertonGame.Server" && dotnet dev-certs https --trust >nul 2>&1 && dotnet run --urls "https://localhost:7191;http://localhost:5062""

popd
echo Server started in new window. Close that window or press Ctrl+C there to stop the server.
pause
