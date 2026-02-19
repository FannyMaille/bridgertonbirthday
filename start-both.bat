@echo off
echo ===========================================
echo Starting BridgertonGame Application
echo ===========================================
echo.
echo Starting Server in 3 seconds...
timeout /t 3 /nobreak >nul

start "BridgertonGame Server" cmd /k "cd BridgertonGame.Server && dotnet run"

echo.
echo Waiting for server to initialize...
timeout /t 10 /nobreak

start "BridgertonGame Client" cmd /k "cd BridgertonGame.Client && dotnet run"

echo.
echo ===========================================
echo Both applications are starting!
echo ===========================================
echo Server: https://localhost:7191
echo Client: https://localhost:7113
echo.
echo Press any key to close this window (apps will continue running)...
pause >nul
